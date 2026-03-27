using Microsoft.EntityFrameworkCore;
using ProjectsPage.Components;
using ProjectsPage.Models;
using ProjectsPage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<ClipboardService>();
builder.Services.AddCookiePolicy(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

builder.Services.AddHttpClient();
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
    options.IncludeSubDomains = true;
    options.Preload = true;
});


builder.Services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

var conString = builder.Configuration.GetConnectionString("ProjectsDb") ??
     throw new InvalidOperationException("Connection string 'ProjectsDb'" +
    " not found.");
builder.Services.AddDbContext<ProjectsDbContext>(options =>
    options.UseSqlServer(conString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.Use((context, next)  =>
{
    context.Response.Headers.Append("Content-Security-Policy", "upgrade-insecure-requests; default-src 'self' wss:; img-src 'self' data:; connect-src https: wss:; frame-ancestors 'self'; frame-src 'self' *; script-src 'self' https://static.cloudflareinsights.com 'sha256-f97Y2Dpz08nctjpmlolnUHCpzBPzgBQNuU9k3U8rCJg=' 'sha256-TlR0wK5VhVNKCRNCjZDLXK4LmS2rxHXS+M6ooMqO//c=' 'sha256-PWJ72QSVNqhMJQvmMDLk9r4IylnqHkLPiWnSe0qVn8k='");
    context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=()");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    
    return next(context);
});

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseWebSockets();
app.UseCookiePolicy();
app.UseSession();
app.UseAntiforgery();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();