// --Copyright (c) 2026 Robert A. Howell

using System.Data;
using System.Text.Json;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Components.Primary;

public partial class FrameSelection
{
    protected override void OnInitialized()
    {
        AllSelections = WebsiteSelections.Concat(DemoSelections)
                                         .OrderByDescending(x => x.SelectionDetails.StartDate)
                                         .ThenBy(x => x.Title)
                                         .ToList();

        WebsiteSelections = FrameSelectionData.WebsiteSelections;

        //On refresh, Frame Selection should match the current url data
        //var relativePath = Nav.ToAbsoluteUri(Nav.Uri).AbsolutePath;
        Nav.LocationChanged += HandleLocationChanged;

        SyncSelectionWithCurrentUrl(Nav.Uri);
    }

    public static List<DomainOption> DomainOptionsData(string projectName)
    {
        ArgumentNullException.ThrowIfNull(projectName);

        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName);

        if (websitesDocsArray == null) throw new DataException("Data fetch is null");

        var websitesData = websitesDocsArray.First();

        return JsonSerializer.Deserialize<List<DomainOption>>(websitesData,
                                                              new JsonSerializerOptions
                                                              { PropertyNameCaseInsensitive = true }) ??
                throw new ApplicationException("WebsitesOptions is null");
    }

    public List<FrameSelectionOption> ConcatWebsiteAndDemoSelections()
    {
        var allSelections = WebsiteSelections.Concat(DemoSelections).ToList();
        allSelections.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.OrdinalIgnoreCase));

        return allSelections;
    }

    public void Dispose()
    {
        Nav.LocationChanged -= HandleLocationChanged;
    }
};
