using Microsoft.AspNetCore.Components;
using ProjectsPage.Domain;

namespace ProjectsPage.Presentation.Primary;

public partial class Database : ComponentBase
{
    public static Queue<AppAnchor> PageLinks = CreateNewLinksStack();

    private static Queue<AppAnchor> CreateNewLinksStack()
    {
        Queue<AppAnchor> databaseTableLinks = new();
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/heartbeat-all",
                              Title = "Heartbeat All",
                              TextContent = "Heartbeat All | roberthowell.dev"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/heartbeat-failures",
                              Title = "Heartbeat Failures",
                              TextContent = "Heartbeat Failures | roberthowell.dev"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/websites/spaceflight-news-app/database",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/websites/randomwebbits/database",
                              Title = "Database | Random Web Bits",
                              TextContent = "Random Web Bits - Accessed By"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/websites/randomwebbits/database2",
                              Title = "Database2 | Random Web Bits",
                              TextContent = "Random Web Bits - Anonymous User"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/websites/randomwebbits/database3",
                              Title = "Database3 | Random Web Bits",
                              TextContent = "Random Web Bits - User Agent"
                      });
        return databaseTableLinks;
    }

    public Queue<AppAnchor> _links = CreateNewLinksStack();
}
