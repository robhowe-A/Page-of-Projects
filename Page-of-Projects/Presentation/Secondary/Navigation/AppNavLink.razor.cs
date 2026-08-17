// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class AppNavLink : ComponentBase
{
    [Parameter] public required DatabaseTableLink Link { get; set; }
}