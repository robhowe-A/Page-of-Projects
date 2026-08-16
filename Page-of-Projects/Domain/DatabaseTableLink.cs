// --Copyright (c) 2026 Robert A. Howell

namespace ProjectsPage.Domain;

public class DatabaseTableLink
{
    public required string Href { get; set; }
    public bool? Data_Enchance_Nav { get; set; } = false;
    public required string Title { get; set; }
    public string? Rel { get; set; } = "opener";
    public string? Target { get; set; } = "_self";
    public required string? TextContent { get; set; }
};
