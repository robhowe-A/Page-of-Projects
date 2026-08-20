// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class AppNavLink : ComponentBase
{
    [Parameter] public required AppAnchor Link { get; set; }
    [Parameter] public string? ClassNames { get; set; }
}