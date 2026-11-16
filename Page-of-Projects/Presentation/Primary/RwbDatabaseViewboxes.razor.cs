// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProjectsPage.Domain;

namespace ProjectsPage.Components.Primary;

public partial class RwbDatabaseViewboxes : ComponentBase
{
    private RwbAccessRecord[] _arRows = [];
    private RwbAnonymousUser[] _auRows = [];
    private RwbUserAgent[] _uaRows = [];

    private bool _isAccessRecordFilterSubmitting;
    private bool _isUserAgentFilterSubmitting;

    private Task OnAccessRecordFilterSubmittingChanged(bool isSubmitting)
    {
        _isAccessRecordFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnUserAgentFilterSubmittingChanged(bool isSubmitting)
    {
        _isUserAgentFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnAccessRecordFilterFindComplete(List<RwbAccessRecord> rows)
    {
        _arRows = rows.ToArray();
        return Task.CompletedTask;
    }

    private Task OnUserAgentFilterFindComplete(List<RwbUserAgent> rows)
    {
        _uaRows = rows.ToArray();
        return Task.CompletedTask;
    }

    private bool? _isArPendingData = true;
    private bool? _isAuPendingData = true;
    private bool? _isUaPendingData = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isArPendingData = true;
            _isAuPendingData = true;
            _isUaPendingData = true;

            await using var dbContext = EntityModels.CreateRwbDbContext();

            try
            {
                _arRows = await dbContext.access_record.AsNoTracking()
                                         .OrderBy(k => k.AnonymousUserId)
                                         .ThenBy(k => k.LastAccessed)
                                         .Where(x => x.AnonymousUserId >= 0)
                                         .Take(500)
                                         .ToArrayAsync();

                _auRows = await dbContext.anonymous_user.AsNoTracking()
                                         .Where(x => x.AnonymousUserId >= 1)
                                         .Take(500)
                                         .ToArrayAsync();

                _uaRows = await dbContext.user_agent.AsNoTracking()
                                         .Where(x => x.AgentId >= 1)
                                         .ToArrayAsync();
            }
            catch (Exception exception)
            {
                throw new ApplicationException(exception.Message);
            }
            finally
            {
                _isArPendingData = false;
                _isAuPendingData = false;
                _isUaPendingData = false;
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}