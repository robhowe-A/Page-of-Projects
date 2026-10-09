//--Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using ProjectsPage.Components.Pages;
using ProjectsPage.Domain;
using ProjectsPage.Pages.Websites;
using ProjectsPage.Presentation.Aside;

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
                              Href = "/websites/spaceflight-news-app/database",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App",
                              AddNavLink = false
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
                                                <tr>
                                                    <td>Id</td>
                                                    <td>TimeStamp</td>
                                                    <td>Domain</td>
                                                    <td>ResponseCode</td>
                                                </tr>
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
                                               <tr>
                                                   <td>Id</td>
                                                   <td>TimeStamp</td>
                                                   <td>Domain</td>
                                                   <td>ResponseCode</td>
                                               </tr>
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
                                               <tr>
                                                   <td>ArticleNum</td>
                                                   <td>Date</td>
                                                   <td>Featured</td>
                                                   <td>NewsSite</td>
                                               </tr>
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
                                                                          <tr>
                                                                              <td>Date</td>
                                                                              <td>Media</td>
                                                                              <td>Title</td>
                                                                          </tr>
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
                                                                          <tr>
                                                                              <td>Anonymous User Id</td>
                                                                              <td>Uri</td>
                                                                              <td>Last Accessed</td>
                                                                          </tr>
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
                                                                          <tr>
                                                                              <td>Digest</td>
                                                                              <td>Last Accessed</td>
                                                                          </tr>
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
                                                                          <td>Id</td>
                                                                          <td>Last Accessed</td>
                                                                          """,
                                       RowTemplateBuildStr = BuildUserAgentRow,
                               }
        ]);
    }
}
