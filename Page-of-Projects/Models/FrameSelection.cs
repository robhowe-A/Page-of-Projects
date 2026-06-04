// --Copyright (c) 2026 Robert A. Howell

using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectsPage.Models;

public enum SortOrder
{
    NEWEST,
    OLDEST,
    AZ,
    ZA
};

public class FrameSelectionOption
{
    [JsonConstructor]
    internal FrameSelectionOption(string href, string title, string innerText, string externalHref,
        string externalTitle, string imageUrl, string imageAltText, List<string>? referenceHrefs,
        SelectionDetails selectionDetails, bool isSelected, Networking networking)
    {
        Href = href ?? throw new ArgumentNullException(nameof(href));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        InnerText = innerText ?? throw new ArgumentNullException(nameof(innerText));
        ExternalHref = externalHref ?? throw new ArgumentNullException(nameof(externalHref));
        ExternalTitle = externalTitle ?? throw new ArgumentNullException(nameof(externalTitle));
        ImageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
        ImageAltText = imageAltText ?? throw new ArgumentNullException(nameof(imageAltText));
        ReferenceHrefs = referenceHrefs;
        SelectionDetails = selectionDetails ?? throw new ArgumentNullException(nameof(selectionDetails));
        IsSelected = isSelected;
        if(ReferenceHrefs != null)
            DocHrefTitles = this.GetDocsHrefTitle(ReferenceHrefs);
        Networking = networking ?? throw new ArgumentNullException(nameof(networking));
    }

    public required string Href { get; init; }
    public required string Title { get; init; }
    public required string InnerText { get; init; }
    public required string ExternalHref { get; init; }
    public required string ExternalTitle { get; init; }
    public required string ImageUrl { get; init; }
    public required string ImageAltText { get; init; }
    public List<string>? ReferenceHrefs { get; init; }
    public required SelectionDetails SelectionDetails { get; init; }
    public bool IsSelected { get; init; } //Not in JSON
    public required Networking Networking { get; init; }
    
    public readonly Dictionary<string, string> DocHrefTitles = [];
    private static readonly Dictionary<string, string> DocHrefTitleCache = new();
    private Dictionary<string, string> GetDocsHrefTitle(List<string> docsHref)
    {
        Dictionary<string, string> hrefs = [];
        using var client = new HttpClient();

        if (this.ReferenceHrefs != null && this.DocHrefTitles.Count == ReferenceHrefs.Count) return this.DocHrefTitles;
        foreach (var href in docsHref)
        {
            var hrefColl = href.Split("|");
            if (DocHrefTitleCache.TryGetValue(href, out var cachedTitle))
            {
                
                hrefs.Add(hrefColl[0], cachedTitle);
                continue;
            }
            var requestUri = new Uri(hrefColl[0], UriKind.Absolute);
#if LOOPBACK
            continue;
#endif
            var response = client.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;

            var link = hrefColl[0];
            string linkTitle;
            // href|Code|<manual title>
            
            if (hrefColl.Length != 1 || (hrefColl.Length > 1 && string.Equals(hrefColl[1], @"TitleOverride")))
            {
                linkTitle = hrefColl[2]; //href is the title
                DocHrefTitleCache[link] = linkTitle;
            }
            else
            {
                linkTitle = "Missing title";
            }
            
            WriteLine($"DocHrefTitles-{requestUri.AbsoluteUri}-{requestUri.HostNameType}->title");
            if ((hrefColl.Length == 1 || (hrefColl.Length > 1 && !string.Equals(hrefColl[1], @"TitleOverride"))))
            {
                //read result to show the html response head
                //filter for <title>
                // href|TitleOverride|<Manual Title>
                var titleRegexMatch = System.Text.RegularExpressions.Regex.Match(
                    response,
                    "<title>(.+?)</title>",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (!titleRegexMatch.Success)
                {
                    hrefs.Add(link, linkTitle);
                    continue;
                }

                //extract title value and return
                linkTitle = titleRegexMatch.Groups[1].Value;
                linkTitle = linkTitle.Replace("&#8212;", "-");
                linkTitle = linkTitle.Replace("&lt;", "<");
                linkTitle = linkTitle.Replace("&gt;", ">");
            }
            DocHrefTitleCache[link] = linkTitle;
            hrefs.Add(link, linkTitle);
        }
        return hrefs;
    }
};

internal sealed class FrameSelectionFetch
{
    public string[] GetWebsitesData(string site)
    {
        return string.IsNullOrWhiteSpace(site) 
            ? throw new ArgumentException("Value cannot be null or whitespace.", nameof(site)) 
            : FetchWebsitesData(site);
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
    
    public string[] SearchIndexTerms
    {
        get
        {
            var context = EntityModels.CreateProjectsDbContext();
            var searchIndexTermsSite = "ProjectsPageSearchIndexTerms";
            var docs = (from j in context.projects where j.Site == searchIndexTermsSite select j.Document);
            
            return docs.ToArray();
        }
    }
};

internal static class FrameSelectionData
{
    public static readonly List<FrameSelectionOption> WebsiteSelections = WebsitesOptionsData(Projects.Websites);
    public static readonly List<FrameSelectionOption> DemoSelections =  WebsitesOptionsData(Projects.Demos);
    
    private static List<FrameSelectionOption> WebsitesOptionsData(string projectName)
    {
        WriteLine($"Loading frame selection data for {projectName}");// Temporary log

        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName );
        if (websitesDocsArray is null) throw new ArgumentNullException(nameof(websitesDocsArray));
        var websitesData = JDocsDataStringLoop(websitesDocsArray);

        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }
    
    private static string JDocsDataStringLoop(string[] docsArr) {

        ArgumentNullException.ThrowIfNull(docsArr);

        var values = "[";
        for (var i = 0; i < docsArr.Length; i++)
        {
            if (i == docsArr.Length - 1)
            {
                values += docsArr[i];
                break;
            }
            values += docsArr[i] + ",";
        }
        values += "]";

        return values;
    }

};