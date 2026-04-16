using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ProjectsPage.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseProjectSecurityHeaders(this WebApplication app)
    {
        app.Use((context, next) =>
        {
#if DEBUG
            context.Response.Headers.Append(
                "Content-Security-Policy",
                "upgrade-insecure-requests; default-src 'self' wss:; img-src 'self' data:; connect-src https: wss:; frame-ancestors 'self'; frame-src 'self' *; script-src 'self' 'unsafe-inline'");
#else
            context.Response.Headers.Append(
                "Content-Security-Policy",
                "upgrade-insecure-requests; default-src 'self' wss:; img-src 'self' data:; connect-src https: wss:; frame-ancestors 'self'; frame-src 'self' *; script-src 'self' https://static.cloudflareinsights.com 'sha256-f97Y2Dpz08nctjpmlolnUHCpzBPzgBQNuU9k3U8rCJg=' 'sha256-TlR0wK5VhVNKCRNCjZDLXK4LmS2rxHXS+M6ooMqO//c=' 'sha256-PWJ72QSVNqhMJQvmMDLk9r4IylnqHkLPiWnSe0qVn8k=' 'sha256-9uKR2oMobB7SIG1VdHGDlonPdBCtzSM8jqLE0yzPa3k=' 'sha256-QvWGboKGmU9tmscoI+YdYRFTRftLGlXGC1ttnpp7DOw=' 'sha256-Jvc0TGT8uqTg7H1/lxh7C0wcChvE2tXatM70n4UZd2k='");
#endif
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=()");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");

            return next(context);
        });

        return app;
    }

    public static IServiceCollection AddProjectsContext( this IServiceCollection services, string? connectionString = null)
    {
        
        if (connectionString == null)
        {
            SqlConnectionStringBuilder builder = new();
            builder.DataSource = string.Format("tcp:{0},{1}",
                System.Environment.GetEnvironmentVariable("ProjectsDb:Host"),
                3306);
            builder.InitialCatalog = "jdoc_store";
            builder.TrustServerCertificate = true;
            builder.MultipleActiveResultSets = true;
            builder.ConnectTimeout = 3;

            builder.UserID = System.Environment.GetEnvironmentVariable("MY_SQL_USR") ?? throw new InvalidOperationException("SQL USER NOT FOUND");
            builder.Password= System.Environment.GetEnvironmentVariable("MY_SQL_PASSWORD") ?? throw new InvalidOperationException("SQL PASSWORD NOT FOUND");
            connectionString = builder.ConnectionString;
        }

        services.AddDbContext<Models.ProjectsDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
            options.LogTo(Console.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
        },
        contextLifetime: ServiceLifetime.Transient,
        optionsLifetime: ServiceLifetime.Transient);

        return services;
    }
};
