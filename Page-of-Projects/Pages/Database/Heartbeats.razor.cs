// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Components.Pages;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Pages.Database;

public partial class Heartbeats : HeartbeatsTableBase
{
    private readonly ProjectsHeartbeatFetch _projectsHeartbeatFetch = new();

    private List<HeartbeatData> HeartbeatsAll => _projectsHeartbeatFetch.ProjectsHeartbeats().ConvertAll(i => new HeartbeatData
        {
                Id = i.Id,
                Timestamp = i.RecordTimestamp,
                Agent = i.Agent,
                Project = i.ProjectName,
                Domain = i.Domain,
                RelativeUrl = i.UrlRel,
                ResponseCode = i.ResponseStatus
        });
};
