// --Copyright (c) 2026 Robert A. Howell

using System.Data;
using ProjectsPage.Domain;
using System.Text.Json;

namespace ProjectsPage.Components.Primary;

public partial class FrameSelection
{

    public static List<DomainOption> DomainOptionsData(string projectName)
    {
        ArgumentNullException.ThrowIfNull(projectName);
        
        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName);
        if(websitesDocsArray == null) throw new DataException("Data fetch is null");
        
        var websitesData = websitesDocsArray.First();

        return JsonSerializer.Deserialize<List<DomainOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }
};
