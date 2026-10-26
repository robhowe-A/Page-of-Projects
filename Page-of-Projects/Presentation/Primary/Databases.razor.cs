//--Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using ProjectsPage.Components.Pages;
using ProjectsPage.Domain;
using ProjectsPage.Pages.Database;
using ProjectsPage.Presentation.Aside;

namespace ProjectsPage.Presentation.Primary;

public partial class Databases : ComponentBase
{
    public static Queue<AppAnchor> PageLinks = CreateNewLinksStack();

    private static Queue<AppAnchor> CreateNewLinksStack()
    {
        Queue<AppAnchor> databaseTableLinks = new();
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/projects-hub/heartbeat-all",
                              Title = "Database | Heartbeat All | Projects Hub",
                              TextContent = "Heartbeat All | roberthowell.dev"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/projects-hub/heartbeat-failures",
                              Title = "Database | Heartbeat Failures | Projects Hub",
                              TextContent = "Heartbeat Failures | roberthowell.dev"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/spaceflight-news-app/sfan-db0",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/spaceflight-news-app/sfan-db0",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App",
                              AddNavLink = false
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits/rwb-db0",
                              Title = "Database | Access Record | Random Web Bits",
                              TextContent = "Random Web Bits - Access Record"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits/rwb-db0",
                              Title = "Database | Anonymous User | Random Web Bits",
                              TextContent = "Random Web Bits - Anonymous User",
                              AddNavLink = false
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits/rwb-db0",
                              Title = "Database | User Agent | Random Web Bits",
                              TextContent = "Random Web Bits - User Agent",
                              AddNavLink = false
                      });
        return databaseTableLinks;
    }

    public Queue<AppAnchor> _links = CreateNewLinksStack();
    public List<DatabaseOverview> Items = new List<DatabaseOverview>();
    private static MarkupString BuildHeartbeatRow(object o)
    {
        var item = (HeartbeatData)o;
            return (MarkupString)$"""
                              <tr>
                                  <td>{item.Id}</td>
                                  <td>{item.Timestamp}</td>
                                  <td>{item.Domain}</td>
                                  <td>{item.ResponseCode}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildAnonymousUserRow(object o)
    {
        var item = (RwbAnonymousUser)o;
            return (MarkupString)$"""
                              <tr>
                                  <td>{item.IpAddrHash}</td>
                                  <td>{item.LastAccessed}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildAccessRecordRow(object o)
    {
        var item = (RwbAccessRecord)o;
        return (MarkupString)$"""
                              <tr>
                                  <td>{item.AnonymousUserId}</td>
                                  <td>{item.Uri}</td>
                                  <td>{item.LastAccessed}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildUserAgentRow(object o)
    {
        var item = (RwbUserAgent)o;
        return (MarkupString)$"""
                              <tr>
                                <td>{item.AgentId}</td>
                                <td>{item.UserAgentFull}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildSpaceflightApodRow(object o)
    {
        var item = (SpaceflightApod)o;
            return (MarkupString)$"""
                              <tr>
                                  <td>{item.Date}</td>
                                  <td>{item.Media}</td>
                                  <td>{item.ApodTitle}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildSpaceflightArticleRow(object o)
    {
        var item = (SpaceflightArticle)o;
            return (MarkupString)$"""
                              <tr>
                                  <td>{item.ArticleNum}</td>
                                  <td>{item.Date}</td>
                                  <td>{item.Featured}</td>
                                  <td>{item.NewsSite}</td>
                              </tr>
                              """;
    }

    private static MarkupString BuildHeartbeatsRow(object o)
    {
        var item = (HeartbeatData)o;
            return (MarkupString)$"""
                               <tr>
                                   <td>{item.Id}</td>
                                   <td>{item.Timestamp}</td>
                                   <td>{item.Domain}</td>
                                   <td>{item.ResponseCode}</td>
                               </tr>
                               """;
    }

    private static MarkupString BuildHeartbeatsFailuresRow(object o)
    {
        var item = (HeartbeatData)o;
            return (MarkupString)$"""
                               <tr>
                                   <td>{item.Id}</td>
                                   <td>{item.Timestamp}</td>
                                   <td>{item.Domain}</td>
                                   <td>{item.ResponseCode}</td>
                               </tr>
                               """;
    }

    protected override void OnInitialized()
    {
        Items.AddRange([
                               new DatabaseOverview()
                               {
                                       OverviewItems =
                                               HeartbeatsTableBase
                                                      .FiveHeartbeats(),
                                       OverviewItemsType = HeartbeatsTableBase.FiveHeartbeats().First().GetType(),
                                       OverviewItemsCsvFileName =
                                               $"heartbeat_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText =
                                               "Download CSV",
                                       CaptionText = "HeartBeats.All",
                                       HeaderTemplateStr = (MarkupString)
                                                """
                                                <thead>
                                                    <th>Id</th>
                                                    <th>TimeStamp</th>
                                                    <th>Domain</th>
                                                    <th>ResponseCode</th>
                                                </thead>
                                                """,
                                       RowTemplateBuildStr = BuildHeartbeatsRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = HeartbeatsTableBase.FiveHeartbeatsFailures(),
                                       OverviewItemsType = HeartbeatsTableBase.FiveHeartbeatsFailures().First().GetType(),
                                       OverviewItemsCsvFileName =
                                               $"heartbeat-failures_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText =
                                               "Download CSV",
                                       CaptionText = "HeartBeats.Failures",
                                       HeaderTemplateStr = (MarkupString)
                                               """
                                               <thead>
                                                   <th>Id</th>
                                                   <th>TimeStamp</th>
                                                   <th>Domain</th>
                                                   <th>ResponseCode</th>
                                               </thead>
                                               """,
                                       RowTemplateBuildStr = BuildHeartbeatsFailuresRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = SNAPI_Db.FiveArticles(),
                                       OverviewItemsType = SNAPI_Db.FiveArticles().First().GetType(),
                                       OverviewItemsCsvFileName = $"spaceflight-articles_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "SpaceFlightNewsApp.Article",
                                       HeaderTemplateStr = (MarkupString)
                                               """
                                               <thead>
                                                   <th>ArticleNum</th>
                                                   <th>Date</th>
                                                   <th>Featured</th>
                                                   <th>NewsSite</th>
                                               </thead>
                                               """,
                                       RowTemplateBuildStr = BuildSpaceflightArticleRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = SNAPI_Db.FiveApods(),
                                       OverviewItemsType = SNAPI_Db.FiveApods().First().GetType(),
                                       OverviewItemsCsvFileName = $"spaceflight-apods_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "SpaceFlightNewsApp.Apod",
                                       HeaderTemplateStr = (MarkupString)
                                                                          """
                                                                          <thead>
                                                                              <th>Date</th>
                                                                              <th>Media</th>
                                                                              <th>Title</th>
                                                                          </thead>
                                                                          """,
                                       RowTemplateBuildStr =  BuildSpaceflightApodRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = RandomWebBitsBase.FiveAccessRecords(),
                                       OverviewItemsType = RandomWebBitsBase.FiveAccessRecords().First().GetType(),
                                       OverviewItemsCsvFileName = $"rwb-accessed-by_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "RandomWebBits.AccessRecord",
                                       HeaderTemplateStr = (MarkupString)
                                                                          """
                                                                          <thead>
                                                                              <th>Anonymous User Id</th>
                                                                              <th>Uri</th>
                                                                              <th>Last Accessed</th>
                                                                          </thead>
                                                                          """,
                                       RowTemplateBuildStr = BuildAccessRecordRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = RandomWebBitsBase.FiveAnonymousUser(),
                                       OverviewItemsType = RandomWebBitsBase.FiveAnonymousUser().First().GetType(),
                                       OverviewItemsCsvFileName = $"rwb-anonymous-user_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "RandomWebBits.AnonymousUser",
                                       HeaderTemplateStr = (MarkupString)
                                                                          """
                                                                          <thead>
                                                                              <th>Digest</th>
                                                                              <th>Last Accessed</th>
                                                                          </thead>
                                                                          """,
                                       RowTemplateBuildStr = BuildAnonymousUserRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = RandomWebBitsBase.FiveUserAgents(),
                                       OverviewItemsType = RandomWebBitsBase.FiveUserAgents().First().GetType(),
                                       OverviewItemsCsvFileName = $"rwb-user-agents_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "RandomWebBits.UserAgent",
                                       HeaderTemplateStr = (MarkupString)
                                                                          """
                                                                          <thead>
                                                                            <th>Id</th>
                                                                            <th>Last Accessed</th>
                                                                          </thead>
                                                                          """,
                                       RowTemplateBuildStr = BuildUserAgentRow,
                               }
        ]);
    }
}
