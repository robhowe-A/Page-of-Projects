// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Mvc;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Jit;

public class JitController : Controller
{
    [HttpPost("/api/jit")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Jit([FromForm] string? username, [FromForm] string? repositoryName,
                                         [FromForm] string? repositoryNumber)
    {
        if (
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(repositoryName) ||
            !GitHubApi.Collaborator.CheckUsernameIsValid(username)
            )
        {
            TempData[$"JitResult-{repositoryNumber}"] = "Not Implemented";
            return LocalRedirect("/jit");
        }

        GitHubApi.Collaborator collaborator = new(repositoryName, username);

#if LOOPBACK
        TempData[$"JitResult"] = "Check Loopback";
        return LocalRedirect("/jit");
#endif

        GitHubApi gitHubApi = new();
        gitHubApi.EnsureClientUsernameKeyVaultAddition(gitHubApi, collaborator);

        var response = await GitHubApiCollaboratorAdditionHttpRequest(gitHubApi, collaborator);

        var responseContent = response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            TempData[$"JitResult-{repositoryNumber}"] = (int)response.StatusCode;
            //TempData[$"JitResult-{repositoryNumber}-response"] = response;
        }
        else
        {
            TempData[$"JitResult-{repositoryNumber}"] = "Not Implemented";
        }

        return LocalRedirect("/jit");
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
