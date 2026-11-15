// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Components.Pages;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Components.Primary;

public partial class ProjectsHubDatabaseViewboxes : HeartbeatsTableBase
{
    private HeartbeatData[]? _heartbeatRows = null;

    private bool _isHeartbeatFilterSubmitting;
    private readonly ProjectsHeartbeatFetch _projectsHeartbeatFetch = new();

    private List<HeartbeatData> HeartbeatsAll = [];

    private bool _isHeartbeatPendingData = true;

    protected override async Task OnInitializedAsync()
    {
        var heartbeats = await _projectsHeartbeatFetch.ProjectsHeartbeats();

        try
        {
            HeartbeatsAll = heartbeats.ConvertAll(i => new HeartbeatData
                                                       {
                                                               Id = i.Id,
                                                               Timestamp = i.RecordTimestamp,
                                                               Agent = i.Agent,
                                                               Project = i.ProjectName,
                                                               Domain = i.Domain,
                                                               RelativeUrl = i.UrlRel,
                                                               ResponseCode = i.ResponseStatus
                                                       });
        }
        catch (Exception exception)
        {
            throw new ApplicationException(exception.Message);
        }
        finally
        {
            _isHeartbeatPendingData = false;
        }
    }

    private Task OnHeartbeatFilterSubmittingChanged(bool isSubmitting)
    {
        _isHeartbeatFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnHeartbeatFilterFindComplete(List<HeartbeatData> rows)
    {
        _heartbeatRows = rows.ToArray();
        return Task.CompletedTask;
    }
}