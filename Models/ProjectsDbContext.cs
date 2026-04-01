using Microsoft.EntityFrameworkCore;
using System.Reflection;

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

        public ProjectsDbContext GetContext()
        {
            //LOGLEAF
            return CreateContext();
        }

        private ProjectsDbContext CreateContext()
        {

#if DEBUG
            //Create a context for this backend request to use
            var iConfig = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets(Assembly.GetExecutingAssembly()).Build();
            string str = iConfig.GetConnectionString("ProjectsDb") ?? string.Empty;

#else
        //Create a context for this backend request to use
        var iConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        string str = System.Environment.GetEnvironmentVariable("ProjectsDb", EnvironmentVariableTarget.Machine) ?? string.Empty;
#endif

            return new ProjectsDbContext(str);
        }
    }
}
