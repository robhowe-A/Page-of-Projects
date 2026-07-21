// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Infrastructure;

namespace ProjectsPage.Domain;

internal static class EntityModels
{
    public static ProjectsDbContext CreateProjectsDbContext()
    {
        return new ProjectsDbContext().GetContext("ProjectsDb");
    }

    public static ProjectsDbContext CreatUsageDbContext()
    {
        return new ProjectsDbContext().GetContext("UsageDb");
    }

    public static SpaceflightDbContext CreateArticlesDbContext()
    {
        return new SpaceflightDbContext().GetContext("SpaceflightDb");
    }
};
