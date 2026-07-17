// --Copyright (c) 2026 Robert A. Howell

using Microsoft.EntityFrameworkCore;
using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class ProjectsDbContext : DbContext
{
    private readonly string? _connectionString;

    public ProjectsDbContext()
    {
    }

    private ProjectsDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    private ProjectsDbContext(DbContextOptions<ProjectsDbContext> options, string connectionString) : base(options)
    {
        _connectionString = connectionString;
    }

    public DbSet<ProjectPage> projects { get; set; }
    public DbSet<Heartbeat> Heartbeat { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseMySQL(_connectionString ??
                                    throw new ArgumentNullException(nameof(_connectionString)));
    }

    public ProjectsDbContext GetContext(string connectionString)
    {
        //LOGLEAF
        return CreateContext(connectionString);
    }

    private static ProjectsDbContext CreateContext(string connectionString) => new (
                                            DbConnectionString.GetDbConnectionString(connectionString)
                                                ?? throw new ArgumentNullException(nameof(connectionString)));
};

internal sealed class DbConnectionString
{
    public static string GetDbConnectionString(string connectionString)
    {
#if DEBUG || LOOPBACK

        //Create a context for this backend request to use
        var iConfig =
                new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets
                        (System.Reflection.Assembly.GetExecutingAssembly()).Build();
        var str = iConfig.GetConnectionString(connectionString) ?? string.Empty;

#else
        //Create a context for this backend request to use
        var iConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        string str = System.Environment.GetEnvironmentVariable(connectionString, EnvironmentVariableTarget.Machine) ??
                     string.Empty;
#endif
        return str ?? throw new NullReferenceException("Connection string is empty.");
    }
};
