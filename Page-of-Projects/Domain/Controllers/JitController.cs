// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Mvc;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Jit;

public class JitController : Controller
{
    [HttpPost("/api/jit")]
    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Jit([FromForm] string? username, [FromForm] string? repositoryName)
    {
        if (
            string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(repositoryName) ||
            !GitHubApi.Collaborator.CheckUsernameIsValid(username)
            )
        {
            TempData["JitResult"] = "Not Implemented";
            return LocalRedirect("/jit");
        }

        GitHubApi.Collaborator collaborator = new(repositoryName, username);

        GitHubApi gitHubApi = new();

        //Ensuring the client username is added to the Key Vaule
        switch (gitHubApi.GetKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey))
        {
            case { HasValue: false }:
                gitHubApi.AddKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey, collaborator.ClientUsernameValue);
                break;
            case { HasValue: true }:
                WriteLine("INFO: Client key already exists in Key Vault.");
                break;
            case null:
                gitHubApi.AddKeyVaultSecret(collaborator.ClientUsernameKeyVaultKey, collaborator.ClientUsernameValue);
                break;
        }

        var webclient = new HttpClient();
        webclient.BaseAddress = gitHubApi.CollaboratorAdditionEndpointUrl(collaborator);

        var collaboratorAdditionRequestMessage = gitHubApi.CollaboratorAdditionCreateRequest();

#if LOOPBACK
        TempData["JitResult"] = "Check Loopback";
        return LocalRedirect("/jit");
#endif
        var response = await webclient.SendAsync(collaboratorAdditionRequestMessage);

        var responseContent = response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            TempData["JitResult"] = response.StatusCode.ToString();
        }
        else
        {
            TempData["JitResult"] = "Not Implemented";
        }

        return LocalRedirect("/jit");
    }
};
