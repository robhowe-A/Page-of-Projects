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
