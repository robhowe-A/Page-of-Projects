// --Copyright (c) 2026 Robert A. Howell

using System.Net;
using Microsoft.AspNetCore.Mvc;
using ProjectsPage.Infrastructure;
using ProjectsPage.Services;

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
            !GitHubVaultApi.Collaborator.CheckUsernameIsValid(username)
            )
        {
            TempData[$"AccessResult-{repositoryNumber}"] = "Invalid username";

            return RedirectToAction("RequestNotValid");
        }

        GitHubVaultApi.Collaborator collaborator = new(repositoryName, username);

#if LOOPBACK
        TempData[$"AccessResult"] = "Check Loopback";
        return LocalRedirect("/private-repository-access-request");
#endif

        GitHubVaultApi gitHubVaultApi = new();
        gitHubVaultApi.EnsureClientUsernameKeyVaultAddition(collaborator);

        var GitHubService =  new GitHubService(_collaboratorContext);
        var (responseModeled, response) = await GitHubService.GetCollaboratorAdditionAsync(
                    gitHubVaultApi.CollaboratorAdditionEndpointUrl(collaborator),                                               
                    gitHubVaultApi.CollaboratorAdditionCreateRequest()
                );

        if (responseModeled != null)
        {
            await GitHubService.SynchronizeGitHubCollaboratorResponseAsync(responseModeled);
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

    
};
