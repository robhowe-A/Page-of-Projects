// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Services;

public interface IGitHubService
{
    Task<(GitHubCollaborator?, HttpResponseMessage)> GetCollaboratorAdditionAsync(Uri baseAddress, HttpRequestMessage requestMessage);
}
