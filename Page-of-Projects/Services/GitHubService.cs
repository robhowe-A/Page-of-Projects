// --Copyright (c) 2026 Robert A. Howell

using System.Text.Json;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Services;

public class GitHubService : IGitHubService
{
    public async Task<(GitHubCollaborator?, HttpResponseMessage)> GetCollaboratorAdditionAsync(Uri baseAddress, HttpRequestMessage requestMessage)
    {
        _httpClient.BaseAddress = baseAddress;
        var response = await _httpClient.SendAsync(requestMessage);

        return (CheckNullOrWhiteSpaceResponseAndSerialize(
            await response.Content.ReadAsStringAsync()), response);
    }

    public async Task SynchronizeGitHubCollaboratorResponseAsync(GitHubCollaborator responseModeled)
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
                _collaboratorContext.Collaborators.Add(CollaboratorEntityFactory.CreateCollaborator(user));
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
            _collaboratorContext.Repositories.Add(CollaboratorEntityFactory.CreateRepository(responseModeled.Repository));
        }
        else
        {
            _collaboratorContext.Entry(existingRepository)
                              .CurrentValues
                              .SetValues(CollaboratorEntityFactory.CreateRepository(responseModeled.Repository));
        }

        var existingRepositoryInvitiation = await _collaboratorContext.RepositoryInvitations
                                                                    .FindAsync(responseModeled.Id);

        if (existingRepositoryInvitiation == null)
        {
            _collaboratorContext.RepositoryInvitations.Add(CollaboratorEntityFactory.CreateInvitation(responseModeled));
        }
        else
        {
            _collaboratorContext.Entry(existingRepositoryInvitiation)
                              .CurrentValues
                              .SetValues(CollaboratorEntityFactory.CreateInvitation(responseModeled));
        }

        var existingRepositoryInviteeLinks = await _collaboratorContext.RepositoryInviteeLinks
                                                                     .FindAsync(responseModeled.Id);

        if (existingRepositoryInviteeLinks == null)
        {
            _collaboratorContext.RepositoryInviteeLinks.Add(CollaboratorEntityFactory.CreateInviteeLink(responseModeled));
        }
        else
        {
            _collaboratorContext.Entry(existingRepositoryInviteeLinks)
                              .CurrentValues
                              .SetValues(CollaboratorEntityFactory.CreateInviteeLink(responseModeled));
        }

        if (responseModeled.Repository.Owner != null)
        {
            var existingRepositoryOwnerLinks = await _collaboratorContext.RepositoryOwnerLinks
                                                                   .FindAsync(responseModeled.Repository.Id, responseModeled.Repository.Owner.Id);

            if (existingRepositoryOwnerLinks == null)
            {
                _collaboratorContext.RepositoryOwnerLinks.Add(CollaboratorEntityFactory.CreateOwnerLink(responseModeled.Repository));
            }
            else
            {
                _collaboratorContext.Entry(existingRepositoryOwnerLinks)
                                  .CurrentValues
                                  .SetValues(CollaboratorEntityFactory.CreateOwnerLink(responseModeled.Repository));
            }
        }

        await _collaboratorContext.SaveChangesAsync();
    }

    public GitHubService(GitHubCollaboratorDbContext collaboratorContext)
    {
        _httpClient = new HttpClient();
        _collaboratorContext = collaboratorContext;
    }

    private readonly HttpClient _httpClient;
    private readonly GitHubCollaboratorDbContext _collaboratorContext;

    private GitHubCollaborator? CheckNullOrWhiteSpaceResponseAndSerialize(string responseString)
    {
        GitHubCollaborator? responseModeled = null;

        if (string.IsNullOrWhiteSpace(responseString)) return null;

        return JsonSerializer.Deserialize<GitHubCollaborator>(
            responseString,
            new JsonSerializerOptions
            {
                    AllowOutOfOrderMetadataProperties = true,
                    PropertyNameCaseInsensitive = true,
                    IncludeFields = true
            }) ?? throw new NullReferenceException("Response resulted in null serialization.");
    }
};
