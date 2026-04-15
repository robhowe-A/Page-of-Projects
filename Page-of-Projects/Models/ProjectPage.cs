using System.ComponentModel.DataAnnotations;

namespace ProjectsPage.Models
{
    internal struct Projects
    {
        public const string Websites = "ProjectsPage";
        public const string Demos = "ProjectsPageDemos";
        public const string Domains = "DomainOptions";
    }
    
    public class ProjectPage
    {
        [Key] public int Id { get; init; }
        public required string Site { get; init; }
        public required string Document { get; init; }
    };

    public class SubselectionMiscellaneous
    {
        public List<string>? Items { get; set; }
        public List<string>? RequirementsOl { get; set; }
    };

    public class SelectionDetails
    {
        public string? Details { get; set; }
        public string? ProgrammingLanguage { get; set; }
        public string? Framework { get; set; }
        public List<string>? SecondaryLanguage { get; set; }
        public List<string>? DetailsList { get; set; }
        public List<string>? Descriptions { get; set; }
        public SubselectionMiscellaneous? Miscellaneous { get; set; }
    };
    
    public class FrameSelectionOption
    {
        public required string Href { get; set; }
        public required string Title { get; set; }
        public required string InnerText { get; set; }
        public required string ExternalHref { get; set; }
        public required string ExternalTitle { get; set; }
        public required string ImageUrl { get; set; }
        public required string ImageAltText { get; set; }
        public required SelectionDetails SelectionDetails { get; set; }
        public bool IsSelected { get; set; } = false;
    };

    internal sealed class FrameSelectionFetch
    {
        public string[]? GetWebsitesData(string site, string? site2)
        {
            if (string.IsNullOrWhiteSpace(site))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(site));
            if (!string.IsNullOrWhiteSpace(site2))
                return FetchProjectsData(site, site2);
            string siteName = site;

            return FetchWebsitesData(siteName);
        }

        private string[] FetchWebsitesData(string site)
        {
            var context = EntityModels.CreateProjectsDbContext();
            var docs = (from j in context.projects where j.Site == site select j.Document);

            return docs.ToArray();
        }
        
        private string[] FetchProjectsData(string site, string site2)
        {
            var context = EntityModels.CreateProjectsDbContext();
            var docs = (from j in context.projects where j.Site == site || j.Site == site2 select j.Document);

            return docs.ToArray();
        }
    };
}
