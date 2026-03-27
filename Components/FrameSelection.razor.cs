using Microsoft.EntityFrameworkCore;
using ProjectsPage.Models;
using System.Reflection;
using System.Text.Json;

namespace ProjectsPage.Components;

public class SubselectionMiscellaneous
{
    public List<string>? Items { get; set; }
    public List<string>? RequirementsOl { get; set; }
}

public class SelectionDetails
{
    public string? Details { get; set; }
    public string? ProgrammingLanguage { get; set; }
    public string? Framework { get; set; }
    public List<string>? SecondaryLanguage { get; set; }
    public List<string>? DetailsList { get; set; }
    public List<string>? Descriptions { get; set; }
    public SubselectionMiscellaneous? Miscellaneous { get; set; }
}

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

public partial class FrameSelection
{
        
    private static ProjectsDbContext CreateContext()
    {

#if DEBUG
        //Create a context for this backend request to use
        var iConfig = new ConfigurationBuilder().AddEnvironmentVariables().AddUserSecrets(Assembly.GetExecutingAssembly()).Build();
        string str = iConfig.GetConnectionString("ProjectsDb") ?? string.Empty;

#else
        //Create a context for this backend request to use
        var iConfig = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        string str = System.Environment.GetEnvironmentVariable("ProjectsDb") ?? string.Empty;
#endif

        return new ProjectsDbContext(str);
    }

    private static string[] GetWebsitesData(string site)
    {
        var context = CreateContext();
        var docs = (from j in context.projects where j.site == site select j.document);
        
        return docs.ToArray();
    }

    private static string JDocsDataStringLoop(string[] docsArr) {

        if (docsArr is null) throw new ArgumentNullException(nameof(docsArr));

        var valuelist = "[";
        for (int i = 0; i < docsArr.Length; i++)
        {
            if (i == docsArr.Length - 1)
            {
                valuelist += docsArr[i].ToString();
                break;
            }
            valuelist += docsArr[i].ToString() + ",";
        }
        valuelist += "]";

        return valuelist;
    }

    public static List<FrameSelectionOption> WebsitesOptionsData()
    {
        var websitesDocsArray = GetWebsitesData("ProjectsPage");
        string websitesData = JDocsDataStringLoop(websitesDocsArray);

        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }

    public static List<FrameSelectionOption> DemosOptionsData()
    {
        var demosDocsArray = GetWebsitesData("ProjectsPageDemos");
        string demosData = JDocsDataStringLoop(demosDocsArray);

        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(demosData,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("DemosOptions is null");
    }
};
