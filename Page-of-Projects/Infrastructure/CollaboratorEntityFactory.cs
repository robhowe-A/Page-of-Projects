// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

public static class CollaboratorEntityFactory
{
    public static RepositoryEntity CreateRepository(Repository repository)
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

    public static RepositoryInvitationEntity CreateInvitation(GitHubCollaborator collaborator)
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

    public static RepositoryInviteeLinkEntity CreateInviteeLink(GitHubCollaborator collaborator)
    {
        return new RepositoryInviteeLinkEntity
               {
                       InvitationId = collaborator.Id,
                       InviteeId = collaborator.Invitee.Id,
                       InviterId = collaborator.Inviter.Id
               };
    }

    public static RepositoryOwnerLinkEntity CreateOwnerLink(Repository repository)
    {
        return new RepositoryOwnerLinkEntity
               {
                       RepositoryId = repository.Id,
                       OwnerId = repository.Owner.Id
               };
    }

    public static CollaboratorEntity CreateCollaborator(GitHubUser user)
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
};
