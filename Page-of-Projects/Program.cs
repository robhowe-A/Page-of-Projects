// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Components;
using ProjectsPage.Services;
using ProjectsPage.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(circuit => circuit.DetailedErrors = true);
builder.Services.AddScoped<ClipboardService>();
builder.Services.AddCookiePolicy(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
    options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(15.0);
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
     throw new InvalidOperationException("Connection string 'ProjectsDb' not found.");

builder.Services.AddProjectsContext(conString);
builder.Services.StartAgentHeartbeat();
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseProjectSecurityHeaders();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseWebSockets();
app.UseCookiePolicy();
app.UseSession();
app.UseAntiforgery();
app.UseResponseCompression();
app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode(defaults => defaults.ContentSecurityFrameAncestorsPolicy = null);
app.Run();
