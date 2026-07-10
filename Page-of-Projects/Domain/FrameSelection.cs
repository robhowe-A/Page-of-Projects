// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Infrastructure;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace ProjectsPage.Domain;

public enum SortOrder
{
    NEWEST,
    OLDEST,
    AZ,
    ZA,
    LangAZ,
    LangZA
}

public class FrameSelectionOption
{
    private static readonly ConcurrentDictionary<string, string> DocHrefTitleCache = new();

    public readonly Dictionary<string, string> DocHrefTitles = [];

    [JsonConstructor]
    internal FrameSelectionOption(string href, string title, string innerText, string externalHref,
                                  string externalTitle, string imageUrl, string imageAltText,
                                  List<string>? referenceHrefs,
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

        if (ReferenceHrefs != null)

            DocHrefTitles = GetDocsHrefTitleAsync(ReferenceHrefs);
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

    //private async Task<Dictionary<string, string>> GetDocsHrefTitleAsync(List<string> docsHref)
    private Dictionary<string, string> GetDocsHrefTitleAsync(List<string> docsHref)
    {
        Dictionary<string, string> hrefs = [];
        using var client = new HttpClient();

        if (ReferenceHrefs != null && DocHrefTitles.Count == ReferenceHrefs.Count) return DocHrefTitles;

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
            var responseResult = client.GetAsync(requestUri).Result.Content.ReadAsStringAsync().Result;

            //var response = client.GetAsync(requestUri);
            //var responseResult = response.Content.ReadAsStringAsync().Result;

            var link = hrefColl[0];
            string linkTitle = string.Empty;

            // href|Code|<manual title>

            if (hrefColl.Length == 3 || 
                (hrefColl.Length > 1 && 
                        (hrefColl[1].Contains(@"TitleOverride") || hrefColl[1].Contains(@"Code")
                )))
            {
                linkTitle = hrefColl[2]; //href is the title
                DocHrefTitleCache[link] = linkTitle;
            }
            else
            {
                linkTitle = "Missing title";
            }

            WriteLine($"DocHrefTitles-{requestUri.AbsoluteUri}-{requestUri.HostNameType}->title");

            if (hrefColl.Length == 1)
            {
                //read result to show the html response head
                //filter for <title>
                // href|TitleOverride|<Manual Title>
                var titleRegexMatch = Regex.Match(
                                                  responseResult,
                                                  "<title>(.+?)</title>",
                                                  RegexOptions.IgnoreCase);

                if (!titleRegexMatch.Success)
                {
                    hrefs.Add(link, linkTitle);

                    continue;
                }

                linkTitle = ParseTitle(titleRegexMatch.Groups[1].Value);
            }

            DocHrefTitleCache[link] = linkTitle;
            hrefs.Add(link, linkTitle);
        }

        return hrefs;

        static string ParseTitle(string linkTitle)
        {
            //extract title value and return
            linkTitle = linkTitle.Replace("&#8212;", "-");
            linkTitle = linkTitle.Replace("&lt;", "<");
            linkTitle = linkTitle.Replace("&gt;", ">");

            return linkTitle;
        }
    }
};

internal static class FrameSelectionData
{
    private static readonly FrameSelectionFetch FsFetch = new ();

    public static readonly List<FrameSelectionOption> WebsiteSelections = WebsitesOptionsData(Projects.Websites);
    public static readonly List<FrameSelectionOption> DemoSelections = WebsitesOptionsData(Projects.Demos);

    private static List<FrameSelectionOption> WebsitesOptionsData(string projectName)
    {
        WriteLine($"Loading frame selection data for {projectName}"); // Temporary log

        var websitesDocsArray = FsFetch.GetWebsitesData(projectName);

        if (websitesDocsArray is null) throw new ArgumentNullException(nameof(websitesDocsArray));
        var websitesData = JDocsDataStringLoop(websitesDocsArray);

        return DeserializeProjectJson(websitesData);
    }

    private static List<FrameSelectionOption> DeserializeProjectJson(string jsonStr)
    {
        List<FrameSelectionOption> selections = [];
        try
        {
            selections = JsonSerializer.Deserialize<List<FrameSelectionOption>>(jsonStr,
                                                                             new JsonSerializerOptions
                                                                             { PropertyNameCaseInsensitive = true }) ??
                    throw new ApplicationException("Json is null");
        }
        catch (Exception e) when (e is JsonException or NotSupportedException)
        {
            WriteLine($"Error: {e}", e);
        }

        return selections;
    }

    public static readonly Dictionary<string, string> ProjectsDataSearchCorpusPlainText = FilterAllProjectsDocumentText();

    private static Dictionary<string, string> FilterAllProjectsDocumentText()
    {
        var projectsDocTexts = FsFetch.GetFullProjectsDocumentText();
        
        //DTO-similar dictionary transform; Db-Id will not match selection id, so using title
        Dictionary<string, string> frameSelectionDataSearchCorpus = new();

        foreach (var (projectId, documentText) in projectsDocTexts)
        {
            //Title is contained in the record, so extract from deserialized json
            var frameSelectionData = DeserializeProjectJson($"[{documentText}]");
            var title = frameSelectionData.FirstOrDefault()?.Title ?? "ProjectsPage";

            //Filter out the string values
            var frameSelectionJsonData = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(documentText);

            var searchText = string.Join("||", ExtractValues(frameSelectionJsonData));

            frameSelectionDataSearchCorpus.TryAdd(title, searchText);
        }
        return frameSelectionDataSearchCorpus;

        static IEnumerable<string> ExtractValues(System.Text.Json.JsonElement element)
        {
            switch (element.ValueKind) {
                //JSON is build on two structures: name/value pairs and arrays. Source: json.org
                //Initial object is unkeyed
                case (System.Text.Json.JsonValueKind.Object):
                    var elem = element.EnumerateObject()
                                      .SelectMany(key =>
                                                  {
                                                      if (key.Name == "ImageUrl" ||
                                                          key.Name == "IsSelected" ||
                                                          key.Name == "Networking" ||
                                                          key.Name == "ImageAltText")
                                                      {
                                                          return [];
                                                      }
                                                      else if (key.Name == "HasDatabase" ||
                                                               key.Name == "IsRelationalDb" ||
                                                               key.Name == "IsCloud")
                                                      {
                                                          return [key.Name.ToString()];
                                                      }
                                                      else
                                                      {
                                                          WriteLine($"{key.Value}");
                                                          return ExtractValues(key.Value);
                                                      }
                                                  });

                    return elem;
                case (System.Text.Json.JsonValueKind.Array):
                    return element.EnumerateArray().SelectMany(values =>
                                                               {
                                                                   WriteLine($"{values}");
                                                                   return ExtractValues(values);
                                                               });
                default:
                    return [element.ToString()];
            }
        }
    }

    private static string JDocsDataStringLoop(string[] docsArr)
    {
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
