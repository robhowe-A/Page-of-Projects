// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Domain;

public partial class InternalNavLink : ComponentBase
{
    [Parameter] public required DatabaseTableLink Link { get; set; }
}