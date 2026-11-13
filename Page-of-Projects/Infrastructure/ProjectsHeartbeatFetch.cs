// --Copyright (c) 2026 Robert A. Howell

using System.Net;
using Microsoft.EntityFrameworkCore;
using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class ProjectsHeartbeatFetch
{
    private const int MaxHeartbeatRecords = 40000;
    private const string TestRecordOnlyPrefix = "TEST-RECORD-ONLY:";
    private const string ProjectNamePrefix = "CLOUD|";
    private const string TestPrefix = "TEST:";
    

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
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Where(p => !p.UrlRel.Contains(TestRecordOnlyPrefix) && !p.UrlRel.Contains(TestPrefix)) 
                  .OrderByDescending(p => p.RecordTimestamp)
                  .ThenByDescending(p => p.Id)
                  .Take(MaxHeartbeatRecords)
                  .ToList();
    }

    private List<Heartbeat> FetchProjectsHeartbeatFailures()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Where(p => p.ResponseStatus != nameof(HttpStatusCode.OK))
                  .Where(p => !p.UrlRel.Contains(TestRecordOnlyPrefix) && !p.UrlRel.Contains(TestPrefix))
                  .OrderByDescending(p => p.RecordTimestamp)
                  .ThenByDescending(p => p.Id)
                  .ToList();
    }

    private int FetchProjectsHeartbeatCloudSuccessCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && p.ProjectName.Contains(ProjectNamePrefix)
                                       && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatCloudTotalCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           p.ProjectName.Contains(ProjectNamePrefix));
    }

    private int FetchProjectsHeartbeatOnPremSuccessCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && !p.ProjectName.Contains(ProjectNamePrefix)
                                        && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatOnPremTotalCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           !p.ProjectName.Contains(ProjectNamePrefix));
    }

    private int FetchProjectsHeartbeatSuccessCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) && p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatUnsuccessfulCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix) &&
                                           !p.ResponseStatus.Contains(nameof(HttpStatusCode.OK)));
    }

    private int FetchProjectsHeartbeatTotalCount()
    {
        using ProjectsDbContext projectsDbContext = EntityModels.CreateProjectsDbContext();

        return projectsDbContext.Heartbeat.AsNoTracking().Count(p => !p.ProjectName.Contains(TestRecordOnlyPrefix));
    }
};
