// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Infrastructure;

namespace ProjectsPage.Domain;

internal static class EntityModels
{
    public static ProjectsDbContext CreateProjectsDbContext()
    {
        return new ProjectsDbContext().GetContext("ProjectsDb");
    }

    public static UsageDbContext CreatUsageDbContext()
    {
        return new UsageDbContext().GetContext("UsageDb");
    }
};
