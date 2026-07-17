// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal sealed class FrameSelectionFetch
{
    private readonly ProjectsDbContext _context = EntityModels.CreateProjectsDbContext();

    public string[] SearchIndexTerms
    {
        get
        {
            var searchIndexTermsSite = "ProjectsPageSearchIndexTerms";
            var docs = from j in _context.projects where j.Site == searchIndexTermsSite select j.Document;

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
        var docs = from j in _context.projects where j.Site == site select j.Document;

        return docs.ToArray();
    }

    private string[] FetchProjectsData(string site, string site2)
    {
        var docs = from j in _context.projects where j.Site == site || j.Site == site2 select j.Document;

        return docs.ToArray();
    }

    public Dictionary<int,string> GetFullProjectsDocumentText()
    {
        var first = FetchProjectsFullText();
        var documentMap = first.ToDictionary(p => p.Id, p => p.Document);

        return documentMap;
    }

    private List<ProjectPage> FetchProjectsFullText()
    {
        return _context.projects
                   .Where(
                          project => project.Site == Projects.Websites || project.Site == Projects.Demos)
                   .ToList();
    }
};
