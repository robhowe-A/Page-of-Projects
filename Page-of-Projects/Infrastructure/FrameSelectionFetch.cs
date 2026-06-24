// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class FrameSelectionFetch
{
    public string[] SearchIndexTerms
    {
        get
        {
            using var context = EntityModels.CreateProjectsDbContext();
            var searchIndexTermsSite = "ProjectsPageSearchIndexTerms";
            var docs = from j in context.projects where j.Site == searchIndexTermsSite select j.Document;

            return docs.ToArray();
        }
    }

    public string[] GetWebsitesData(string site)
    {
        return string.IsNullOrWhiteSpace(site)
                ? throw new ArgumentException("Value cannot be null or whitespace.", nameof(site))
                : FetchWebsitesData(site);
    }

    private string[] FetchWebsitesData(string site)
    {
        using var context = EntityModels.CreateProjectsDbContext();
        var docs = from j in context.projects where j.Site == site select j.Document;

        return docs.ToArray();
    }

    private string[] FetchProjectsData(string site, string site2)
    {
        using var context = EntityModels.CreateProjectsDbContext();
        var docs = from j in context.projects where j.Site == site || j.Site == site2 select j.Document;

        return docs.ToArray();
    }

    public int FetchProjectsHeartbeatCloudSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && p.ProjectName.Contains("CLOUD|") 
                                       && p.ResponseStatus.Contains("OK"));
    }

    public int FetchProjectsHeartbeatCloudTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && p.ProjectName.Contains("CLOUD|"));
    }

    public int FetchProjectsHeartbeatOnPremSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && !p.ProjectName.Contains("CLOUD|") 
                                       && p.ResponseStatus.Contains("OK"));
    }

    public int FetchProjectsHeartbeatOnPremTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && !p.ProjectName.Contains("CLOUD|"));
    }

    public int FetchProjectsHeartbeatSuccessCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && p.ResponseStatus.Contains("OK"));
    }

    public int FetchProjectsHeartbeatUnsuccessfulCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:") && !p.ResponseStatus.Contains("OK"));
    }

    public int FetchProjectsHeartbeatTotalCount()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext() ;

        return db.Heartbeat.Count(p => !p.ProjectName.Contains("TEST-RECORD-ONLY:"));
    }
};