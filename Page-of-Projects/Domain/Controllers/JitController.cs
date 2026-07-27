// --Copyright (c) 2026 Robert A. Howell

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjectsPage.Domain;
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

        var responseModeled = JsonSerializer.Deserialize<GitHubCollaborator>(
            await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
              {
                      AllowOutOfOrderMetadataProperties = true,
                      PropertyNameCaseInsensitive = true,
                      IncludeFields = true
              });
        if (responseModeled == null)
        {
            throw new NotImplementedException();
        }

        if (response.IsSuccessStatusCode)
        {
            TempData[$"JitResult-{repositoryNumber}"] = (int)response.StatusCode;
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
