using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ProjectsPage.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseProjectSecurityHeaders(this WebApplication app)
    {
        app.Use((context, next) =>
        {
            context.Response.Headers.Append(
                "Content-Security-Policy",
                "upgrade-insecure-requests; default-src 'self' wss:; img-src 'self' data:; connect-src https: wss:; frame-ancestors 'self'; frame-src 'self' *; script-src 'self' https://static.cloudflareinsights.com 'sha256-f97Y2Dpz08nctjpmlolnUHCpzBPzgBQNuU9k3U8rCJg=' 'sha256-TlR0wK5VhVNKCRNCjZDLXK4LmS2rxHXS+M6ooMqO//c=' 'sha256-PWJ72QSVNqhMJQvmMDLk9r4IylnqHkLPiWnSe0qVn8k='");

            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=()");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("X-Frame-Options", "DENY");

            return next(context);
        });

        return app;
    }
}