// --Copyright (c) 2026 Robert A. Howell

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Access;

public class AccessController : Controller
{
    private readonly GitHubCollaboratorDbContext _collaboratorContext;

    public AccessController(GitHubCollaboratorDbContext collaboratorContext)
    {
        _collaboratorContext = collaboratorContext;
    }

    [HttpGet("/repository-access/invalid-user")]
    public IActionResult RequestNotValid()
    {
        return RedirectPermanent("/private-repository-access-request");
    }

    [HttpPost("/repository-access/collaborator")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Collaborator([FromForm] string? username, [FromForm] string? repositoryName,
                                         [FromForm] string? repositoryNumber)
    {
        if (
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(repositoryName) ||
            !GitHubApi.Collaborator.CheckUsernameIsValid(username)
            )
        {
            TempData[$"AccessResult-{repositoryNumber}"] = "Invalid username";

            return RedirectToAction("RequestNotValid");
        }

        GitHubApi.Collaborator collaborator = new(repositoryName, username);

#if LOOPBACK
        TempData[$"AccessResult"] = "Check Loopback";
        return LocalRedirect("/private-repository-access-request");
#endif

        GitHubApi gitHubApi = new();
        gitHubApi.EnsureClientUsernameKeyVaultAddition(gitHubApi, collaborator);

        var response = await GitHubApiCollaboratorAdditionHttpRequest(gitHubApi, collaborator);
        var responseString = await response.Content.ReadAsStringAsync();
        GitHubCollaborator? responseModeled = null;
        if(!string.IsNullOrWhiteSpace(responseString))
            responseModeled = JsonSerializer.Deserialize<GitHubCollaborator>(
                responseString,
                new JsonSerializerOptions
                  {
                          AllowOutOfOrderMetadataProperties = true,
                          PropertyNameCaseInsensitive = true,
                          IncludeFields = true
                  });

        if (response.IsSuccessStatusCode && responseModeled != null)
        {
            await InsertGitHubCollaboratorResponse(responseModeled);
        }

        if (response.IsSuccessStatusCode)
        {
            TempData[$"AccessResult-{repositoryNumber}"] = response.StatusCode switch
            {
                    HttpStatusCode.Created => "201 Created",
                    HttpStatusCode.NoContent => "204 No Content",
                    _ => "Not Implemented"
            };
        }
        else
        {
            TempData[$"AccessResult-{repositoryNumber}"] = response.StatusCode switch
            {
                    HttpStatusCode.NotFound => "404 User Not Found",
                    HttpStatusCode.UnprocessableContent => "422 Unprocessable",
                    _ => "Not Implemented"
            };
        }

        return LocalRedirect("/private-repository-access-request");
    }

    private async Task InsertGitHubCollaboratorResponse(GitHubCollaborator responseModeled)
    {
        if (responseModeled.Repository == null ||
            responseModeled.Invitee == null ||
            responseModeled.Inviter == null)
        {
            return;
        }
        var collaboratorUsers = new[]
                                {
                                        responseModeled.Invitee,
                                        responseModeled.Inviter,
                                        responseModeled.Repository.Owner
                                }
                               .Where(user => user != null)
                               .GroupBy(user => user.Id)
                               .Select(group => group.First());

        foreach (var user in collaboratorUsers)
        {
            var existing = await _collaboratorContext.Collaborators
                                                   .FindAsync(user.Id);

            if (existing == null)
            {
                _collaboratorContext.Collaborators.Add(ToCollaboratorEntity(user));
            }
            else
            {
                _collaboratorContext.Entry(existing).CurrentValues.SetValues(user);
            }
        }

        var existingRepository = await _collaboratorContext.Repositories
                                                         .FindAsync(responseModeled.Repository.Id);

        if (existingRepository == null)
        {
            _collaboratorContext.Repositories.Add(ToRepositoryEntity(responseModeled.Repository));
        }
        else
        {
            _collaboratorContext.Entry(existingRepository)
                              .CurrentValues
                              .SetValues(ToRepositoryEntity(responseModeled.Repository));
        }

        var existingRepositoryInvitiation = await _collaboratorContext.RepositoryInvitations
                                                                    .FindAsync(responseModeled.Id);

        if (existingRepositoryInvitiation == null)
        {
            _collaboratorContext.RepositoryInvitations.Add(ToRepositoryInvitationEntity(responseModeled));
        }
        else
        {
            _collaboratorContext.Entry(existingRepositoryInvitiation)
                              .CurrentValues
                              .SetValues(ToRepositoryInvitationEntity(responseModeled));
        }

        var existingRepositoryInviteeLinks = await _collaboratorContext.RepositoryInviteeLinks
                                                                     .FindAsync(responseModeled.Id);

        if (existingRepositoryInvitiation == null)
        {
            _collaboratorContext.RepositoryInviteeLinks.Add(ToRepositoryInviteeLinkEntity(responseModeled));
        }
        else
        {
            _collaboratorContext.Entry(existingRepositoryInviteeLinks)
                              .CurrentValues
                              .SetValues(ToRepositoryInviteeLinkEntity(responseModeled));
        }

        if (responseModeled.Repository.Owner != null)
        {
            var existingRepositoryOwnerLinks = await _collaboratorContext.RepositoryOwnerLinks
                                                                   .FindAsync(responseModeled.Repository.Id, responseModeled.Repository.Owner.Id);

            if (existingRepositoryOwnerLinks == null)
            {
                _collaboratorContext.RepositoryOwnerLinks.Add(ToRepositoryOwnerLinkEntity(responseModeled.Repository));
            }
            else
            {
                _collaboratorContext.Entry(existingRepositoryOwnerLinks)
                                  .CurrentValues
                                  .SetValues(ToRepositoryOwnerLinkEntity(responseModeled.Repository));
            }
        }

        await _collaboratorContext.SaveChangesAsync();
    }

    private static RepositoryEntity ToRepositoryEntity(Repository repository)
    {
        return new RepositoryEntity
               {
                       RepositoryId = repository.Id,
                       NodeId = repository.NodeId,
                       Name = repository.Name,
                       FullName = repository.FullName,
                       IsPrivate = repository.Private,
                       HtmlUrl = repository.HtmlUrl,
                       ApiUrl = repository.Url,
                       Description = repository.Description
               };
    }

    private static RepositoryInvitationEntity ToRepositoryInvitationEntity(GitHubCollaborator collaborator)
    {
        return new RepositoryInvitationEntity
               {
                       InvitationId = collaborator.Id,
                       NodeId = collaborator.NodeId,
                       RepositoryId = collaborator.Repository.Id,
                       InviteeId = collaborator.Invitee.Id,
                       InviterId = collaborator.Inviter.Id,
                       Permissions = collaborator.Permissions,
                       CreatedAt = collaborator.CreatedAt.DateTime,
                       ApiUrl = collaborator.Url,
                       HtmlUrl = collaborator.HtmlUrl
               };
    }

    private static RepositoryInviteeLinkEntity ToRepositoryInviteeLinkEntity(GitHubCollaborator collaborator)
    {
        return new RepositoryInviteeLinkEntity
               {
                       InvitationId = collaborator.Id,
                       InviteeId = collaborator.Invitee.Id,
                       InviterId = collaborator.Inviter.Id
               };
    }

    private static RepositoryOwnerLinkEntity ToRepositoryOwnerLinkEntity(Repository repository)
    {
        return new RepositoryOwnerLinkEntity
               {
                       RepositoryId = repository.Id,
                       OwnerId = repository.Owner.Id
               };
    }

    private static CollaboratorEntity ToCollaboratorEntity(GitHubUser user)
    {
        return new CollaboratorEntity
               {
                       UserId = user.Id,
                       Login = user.Login,
                       NodeId = user.NodeId,
                       AvatarUrl = user.AvatarUrl,
                       HtmlUrl = user.HtmlUrl,
                       ApiUrl = user.Url,
                       Type = user.Type,
                       SiteAdmin = user.SiteAdmin
               };
    }

    private async Task<HttpResponseMessage> GitHubApiCollaboratorAdditionHttpRequest(GitHubApi gitHubApi, GitHubApi.Collaborator collaborator)
    {
        var webclient = new HttpClient();
        webclient.BaseAddress = gitHubApi.CollaboratorAdditionEndpointUrl(collaborator);

        var collaboratorAdditionRequestMessage = gitHubApi.CollaboratorAdditionCreateRequest();

        var response = await webclient.SendAsync(collaboratorAdditionRequestMessage);

        return response;
    }
};
