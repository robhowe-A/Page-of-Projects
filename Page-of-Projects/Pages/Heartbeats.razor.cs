// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Components.Pages;

public partial class Heartbeats : ComponentBase
{
    private readonly ProjectsHeartbeatFetch _projectsHeartbeatFetch = new();

    private List<Heartbeat> HeartbeatsAll => _projectsHeartbeatFetch.ProjectsHeartbeats();
};
