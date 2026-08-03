// --Copyright (c) 2026 Robert A. Howell

using Microsoft.EntityFrameworkCore;
using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

public sealed class GitHubCollaboratorDbContext : DbContext
{
    public GitHubCollaboratorDbContext(DbContextOptions<GitHubCollaboratorDbContext> options)
            : base(options)
    {
    }
    public DbSet<CollaboratorEntity> Collaborators => Set<CollaboratorEntity>();
    public DbSet<RepositoryEntity> Repositories => Set<RepositoryEntity>();
    public DbSet<RepositoryInvitationEntity> RepositoryInvitations => Set<RepositoryInvitationEntity>();
    public DbSet<RepositoryInviteeLinkEntity> RepositoryInviteeLinks => Set<RepositoryInviteeLinkEntity>();
    public DbSet<RepositoryOwnerLinkEntity> RepositoryOwnerLinks => Set<RepositoryOwnerLinkEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<RepositoryOwnerLinkEntity>()
                    .HasKey(link => new
                                    {
                                            link.RepositoryId,
                                            link.OwnerId
                                    });

        modelBuilder.Entity<CollaboratorEntity>()
                    .Property(entity => entity.SiteAdmin)
                    .HasColumnType("bit(1)");

        modelBuilder.Entity<RepositoryEntity>()
                    .Property(entity => entity.IsPrivate)
                    .HasColumnType("bit(1)");
    }
};
