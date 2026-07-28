// --Copyright (c) 2026 Robert A. Howell

using System.Net;
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
            TempData[$"JitResult-{repositoryNumber}"] = "Invalid username";
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

        if (response.IsSuccessStatusCode)
        {
            TempData[$"JitResult-{repositoryNumber}"] = response.StatusCode switch
            {
                    HttpStatusCode.Created => "201 Created",
                    HttpStatusCode.NoContent => "204 No Content",
                    _ => "Not Implemented"
            };
        }
        else
        {
            TempData[$"JitResult-{repositoryNumber}"] = response.StatusCode switch
            {
                    HttpStatusCode.NotFound => "404 User Not Found",
                    HttpStatusCode.UnprocessableContent => "422 Unprocessable",
                    _ => "Not Implemented"
            };
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
