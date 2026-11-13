// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Infrastructure;

namespace ProjectsPage.Domain;

internal static class EntityModels
{
    public static ProjectsDbContext CreateProjectsDbContext()
    {
        return ProjectsDbContext.GetContext("ProjectsDb");
    }

    public static ProjectsDbContext CreatUsageDbContext()
    {
        return ProjectsDbContext.GetContext("UsageDb");
    }

    public static SpaceflightDbContext CreateArticlesDbContext()
    {
        return SpaceflightDbContext.GetContext("SpaceflightDb");
    }

    public static RwbDbContext CreateRwbDbContext()
    {
        return RwbDbContext.GetContext("RwbDbContext");
    }
};
