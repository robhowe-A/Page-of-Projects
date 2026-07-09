// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class ProjectsHeartbeatFetch
{
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

    private int FetchProjectsHeartbeatCloudSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && p.ProjectName.Contains("CLOUD|")
                                       && p.ResponseStatus.Contains("OK"));
    }

    private int FetchProjectsHeartbeatCloudTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") &&
                                          p.ProjectName.Contains("CLOUD|"));
    }

    private int FetchProjectsHeartbeatOnPremSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && !p.ProjectName.Contains("CLOUD|")
                                       && p.ResponseStatus.Contains("OK"));
    }

    private int FetchProjectsHeartbeatOnPremTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") &&
                                          !p.ProjectName.Contains("CLOUD|"));
    }

    private int FetchProjectsHeartbeatSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && p.ResponseStatus.Contains("OK"));
    }

    private int FetchProjectsHeartbeatUnsuccessfulCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") &&
                                          !p.ResponseStatus.Contains("OK"));
    }

    private int FetchProjectsHeartbeatTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:"));
    }
};
