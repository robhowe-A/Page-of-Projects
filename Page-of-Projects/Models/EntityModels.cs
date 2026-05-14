// --Copyright (c) 2026 Robert A. Howell

namespace ProjectsPage.Models
{
    public static class EntityModels
    {
        public static ProjectsDbContext CreateProjectsDbContext()
        {
            return new ProjectsDbContext().GetContext();
        }
    }
}
