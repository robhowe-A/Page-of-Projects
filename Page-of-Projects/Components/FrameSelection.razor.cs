using Microsoft.EntityFrameworkCore;
using ProjectsPage.Models;
using System.Reflection;
using System.Text.Json;

namespace ProjectsPage.Components;



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

    public List<FrameSelectionOption> WebsitesOptionsData(string projectName)
    {
        var websitesDocsArray = new FrameSelectionFetch().GetWebsitesData(projectName);
        string websitesData = JDocsDataStringLoop(websitesDocsArray);
        
        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(websitesData,
                   options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("WebsitesOptions is null");
    }

    public List<FrameSelectionOption> DemosOptionsData()
    {
        var demosDocsArray = new FrameSelectionFetch().GetWebsitesData("ProjectsPageDemos");
        string demosData = JDocsDataStringLoop(demosDocsArray);

        return JsonSerializer.Deserialize<List<FrameSelectionOption>>(demosData,
                   new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ??
               throw new ApplicationException("DemosOptions is null");
    }
};
