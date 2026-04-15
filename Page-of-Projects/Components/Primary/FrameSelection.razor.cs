using Microsoft.EntityFrameworkCore;
using ProjectsPage.Models;
using System.Reflection;
using System.Text.Json;
using ProjectsPage.Components.Pages;

namespace ProjectsPage.Components.Primary;

public partial class FrameSelection
{

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

    public List<FrameSelectionOption> WebsitesOptionsData(string projectName, string projectName2)
    {
        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName, String.IsNullOrEmpty(projectName2) ? null : projectName2 );
        string websitesData = JDocsDataStringLoop(websitesDocsArray);
        
        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }
    
    public List<DomainOption> DomainOptionsData(string projectName)
    {
        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName, null );
        string websitesData = websitesDocsArray.First().ToString();
        
        return JsonSerializer.Deserialize<List<DomainOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }
};
