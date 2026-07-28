// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Mvc;

namespace ProjectsPage.Domain;

public class TestController : Controller
{
    [HttpGet("/env")]
    public IActionResult Env()
    {
        return Ok(new {
          Tenant = Environment.GetEnvironmentVariable("AZURE_TENANT_ID"),
          Client = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID"),
          Secret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET") != null
                  ? "Loaded"
                  : "Missing"
        });
    }
};
