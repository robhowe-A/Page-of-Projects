// --Copyright (c) 2026 Robert A. Howell

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectsPage.Domain;

[Table("Collaborators")]
public sealed class CollaboratorEntity
{
    [Key] [Column("UserId")] public long UserId { get; init; }

    [Column("Login")] [MaxLength(256)] public string? Login { get; init; }

    [Column("NodeId")] [MaxLength(64)] public string? NodeId { get; init; }

    [Column("AvatarUrl")] [MaxLength(512)] public string? AvatarUrl { get; init; }

    [Column("HtmlUrl")] [MaxLength(512)] public string? HtmlUrl { get; init; }

    [Column("ApiUrl")] [MaxLength(512)] public string? ApiUrl { get; init; }

    [Column("Type")] [MaxLength(64)] public string? Type { get; init; }

    [Column("SiteAdmin")] public bool? SiteAdmin { get; init; }
};

[Table("Repositories")]
public sealed class RepositoryEntity
{
    [Key] [Column("RepositoryId")] public long RepositoryId { get; init; }

    [Column("NodeId")] [MaxLength(64)] public string? NodeId { get; init; }

    [Column("Name")] [MaxLength(256)] public string? Name { get; init; }

    [Column("FullName")] [MaxLength(256)] public string? FullName { get; init; }

    [Column("IsPrivate")] public bool? IsPrivate { get; init; }

    [Column("HtmlUrl")] [MaxLength(512)] public string? HtmlUrl { get; init; }

    [Column("ApiUrl")] [MaxLength(512)] public string? ApiUrl { get; init; }

    [Column("Description")] public string? Description { get; init; }
};

[Table("RepositoryInvitations")]
public sealed class RepositoryInvitationEntity
{
    [Key] [Column("InvitationId")] public long InvitationId { get; init; }

    [Column("NodeId")] [MaxLength(64)] public string? NodeId { get; init; }

    [Column("RepositoryId")] public long RepositoryId { get; init; }

    [Column("InviteeId")] public long InviteeId { get; init; }

    [Column("InviterId")] public long InviterId { get; init; }

    [Column("Permissions")]
    [MaxLength(32)]
    public string? Permissions { get; init; }

    [Column("CreatedAt")] public DateTime? CreatedAt { get; init; }

    [Column("ApiUrl")] [MaxLength(512)] public string? ApiUrl { get; init; }

    [Column("HtmlUrl")] [MaxLength(512)] public string? HtmlUrl { get; init; }
};

[Table("RepositoryInviteeLinks")]
public sealed class RepositoryInviteeLinkEntity
{
    [Key] [Column("InvitationId")] public long InvitationId { get; init; }

    [Column("InviteeId")] public long InviteeId { get; init; }

    [Column("InviterId")] public long InviterId { get; init; }

};

[Table("RepositoryOwnerLinks")]
public sealed class RepositoryOwnerLinkEntity
{
    [Column("RepositoryId")]
    public long RepositoryId { get; init; }

    [Column("OwnerId")]
    public long OwnerId { get; init; }
};
