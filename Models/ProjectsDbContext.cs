using Microsoft.EntityFrameworkCore;

namespace ProjectsPage.Models
{
    public class ProjectsDbContext : DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options)
        {

        }
    }
}
