// --Copyright (c) 2026 Robert A. Howell

using System.Net;
using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class ProjectsHeartbeatFetch
{
    private const int MaxHeartbeatRecords = 40000;
    private const string TestRecordOnlyPrefix = "TEST-RECORD-ONLY:";
    private const string ProjectNamePrefix = "CLOUD|";
    private const string TestPrefix = "TEST:";
    
    private readonly ProjectsDbContext _db = EntityModels.CreateProjectsDbContext();

    public int ProjectsHeartbeatSuccessCount()
    {
        return FetchProjectsHeartbeatSuccessCount();
    }

    public int ProjectsHeartbeatUnsuccessfulCount()
    {
        return FetchProjectsHeartbeatUnsuccessfulCount();
    }

    public decimal ProjectsHeartbeatSuccessPercentage()
    {
        var successes = FetchProjectsHeartbeatSuccessCount();
        var total = FetchProjectsHeartbeatTotalCount();

        if (total == 0)
            return 0;

        return (Convert.ToDecimal(successes) / total) * 100;
    }

    public decimal ProjectsHeartbeatCloudSuccessPercentage()
    {
        var successes = FetchProjectsHeartbeatCloudSuccessCount();
        var total = FetchProjectsHeartbeatCloudTotalCount();

        return total == 0 ? 0 : (Convert.ToDecimal(successes) / total) * 100;
    }

    public decimal ProjectsHeartbeatOnPremSuccessPercentage()
    {
        var successes = FetchProjectsHeartbeatOnPremSuccessCount();
        var total = FetchProjectsHeartbeatOnPremTotalCount();

        return total == 0 ? 0 : (Convert.ToDecimal(successes) / total) * 100;
    }

    public List<Heartbeat> ProjectsHeartbeatFailures()
    {
        return FetchProjectsHeartbeatFailures();
    }

    public List<Heartbeat> ProjectsHeartbeats()
    {
        return FetchProjectsHeartbeats();
    }

    private List<Heartbeat> FetchProjectsHeartbeats()
    {
        return _db.Heartbeat.Where(p => !p.UrlRel.Contains(TestRecordOnlyPrefix) && !p.UrlRel.Contains(TestPrefix)) 
                  .OrderByDescending(p => p.RecordTimestamp)
                  .ThenByDescending(p => p.Id)
                  .Take(MaxHeartbeatRecords)
                  .ToList();
    }

    private List<Heartbeat> FetchProjectsHeartbeatFailures()
    {
        return _db.Heartbeat.Where(p => p.ResponseStatus != nameof(HttpStatusCode.OK))
                  .Where(p => !p.UrlRel.Contains(TestRecordOnlyPrefix) && !p.UrlRel.Contains(TestPrefix))
                  .OrderByDescending(p => p.RecordTimestamp)
                  .ThenByDescending(p => p.Id)
                  .ToList();
    }

    private int FetchProjectsHeartbeatCloudSuccessCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && p.ProjectName.Contains(ProjectNamePrefix)
                                       && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatCloudTotalCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           p.ProjectName.Contains(ProjectNamePrefix));
    }

    private int FetchProjectsHeartbeatOnPremSuccessCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && !p.ProjectName.Contains(ProjectNamePrefix)
                                        && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatOnPremTotalCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           !p.ProjectName.Contains(ProjectNamePrefix));
    }

    private int FetchProjectsHeartbeatSuccessCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatUnsuccessfulCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           !p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatTotalCount()
    {
        return _db.Heartbeat.Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix));
    }
};
