// --Copyright (c) 2026 Robert A. Howell

namespace ProjectsPage.Domain;

public class AppAnchor
{
    public required string Href { get; set; }
    public bool? DataEnhanceNav { get; set; } = true;
    public bool IsDataEnhanceNav => DataEnhanceNav ?? false;
    public required string Title { get; set; }
    public string? Rel { get; set; } = "opener";
    public string? Target { get; set; } = "_self";
    public required string? TextContent { get; set; }
};
