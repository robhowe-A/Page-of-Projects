using Microsoft.EntityFrameworkCore;

namespace ProjectsPage.Models
{
    public class ProjectsDbContext : DbContext
    {
        public DbSet<Class> projects { get; set; } = default;

        private readonly string _connectionString;

        public ProjectsDbContext()
        {

        }

        public ProjectsDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options, string connectionString) : base(options)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString ?? throw new ArgumentNullException(nameof(_connectionString)));
        }
    }
}
