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

    private List<HeartbeatData> _heartbeatsAll = [];
    private List<HeartbeatData> _heartbeats1000 = [];
    private string LimitWarning { get; set; } = string.Empty;
    private uint QueryLimit { get; set; } = 1000;

    private bool? _isHeartbeatPendingData = true; //state=1,pending data

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isHeartbeatPendingData = true;
            var heartbeats = await _projectsHeartbeatFetch.ProjectsHeartbeats();

            try
            {
                _heartbeatsAll = heartbeats.ConvertAll(i => new HeartbeatData
                                                           {  //DTO
                                                                   Id = i.Id,
                                                                   Timestamp = i.RecordTimestamp,
                                                                   Agent = i.Agent,
                                                                   Project = i.ProjectName,
                                                                   Domain = i.Domain,
                                                                   RelativeUrl = i.UrlRel,
                                                                   ResponseCode = i.ResponseStatus
                                                           });
                _heartbeats1000 = _heartbeatsAll
                                    .OrderByDescending(i => i.Id)
                                    .Take(Convert.ToInt16(QueryLimit))
                                    .ToList();
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                _isHeartbeatPendingData = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }

    private Task OnHeartbeatFilterSubmittingChanged(bool isSubmitting)
    {
        _isHeartbeatFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnHeartbeatFilterFindComplete((List<HeartbeatData> rows, bool isLimitReached) userQuery)
    {
        _heartbeatRows = userQuery.rows.Take(Convert.ToInt16(QueryLimit)).ToArray(); // Limit placement
        LimitWarning = userQuery.isLimitReached ? $"Results view is limited to {QueryLimit} rows." : string.Empty;
        return Task.CompletedTask;
    }
}
