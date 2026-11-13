//--Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProjectsPage.Components.Pages;
using ProjectsPage.Domain;
using ProjectsPage.Presentation.Aside;

namespace ProjectsPage.Components.Primary;

public partial class Databases : ComponentBase
{
    public static Queue<AppAnchor> PageLinks = CreateNewLinksStack();

    private static Queue<AppAnchor> CreateNewLinksStack()
    {
        Queue<AppAnchor> databaseTableLinks = new();
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/projects-hub/heartbeats",
                              Title = "Database | Heartbeat All | Projects Hub",
                              TextContent = "Heartbeat All | roberthowell.dev"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/spaceflight-news-app",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/spaceflight-news-app",
                              Title = "Database | SpaceFlight News App",
                              TextContent = "SpaceFlight News App",
                              AddNavLink = false
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits",
                              Title = "Database | Access Record | Random Web Bits",
                              TextContent = "Random Web Bits - Access Record"
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits",
                              Title = "Database | Anonymous User | Random Web Bits",
                              TextContent = "Random Web Bits - Anonymous User",
                              AddNavLink = false
                      });
        databaseTableLinks.Enqueue(new AppAnchor
                      {
                              Href = "/database/randomwebbits",
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
    private static List<SpaceflightArticle> _fiveArticles = [];
    private static List<SpaceflightApod> _fiveApods = [];
    protected override void OnInitialized()
    {
        using var context = EntityModels.CreateArticlesDbContext();

        _fiveArticles = context.Article.AsNoTracking()
                                    .OrderByDescending(a => a.Date)
                                    .Take(5).ToList();

        _fiveApods = context.APOD.AsNoTracking()
                                 .OrderByDescending(a => a.Date)
                                 .Take(5).ToList();
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
                                                    <tr>
                                                        <th>Id</th>
                                                        <th>TimeStamp</th>
                                                        <th>Domain</th>
                                                        <th>ResponseCode</th>
                                                    </tr>
                                                </thead>
                                                """,
                                       RowTemplateBuildStr = BuildHeartbeatsRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = _fiveArticles,
                                       OverviewItemsType = _fiveArticles.First().GetType(),
                                       OverviewItemsCsvFileName = $"spaceflight-articles_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "SpaceFlightNewsApp.Article",
                                       HeaderTemplateStr = (MarkupString)
                                               """
                                               <thead>
                                                <tr>
                                                    <th>ArticleNum</th>
                                                    <th>Date</th>
                                                    <th>Featured</th>
                                                    <th>NewsSite</th>
                                                </tr>
                                               </thead>
                                               """,
                                       RowTemplateBuildStr = BuildSpaceflightArticleRow,
                               },
                               new DatabaseOverview()
                               {
                                       OverviewItems = _fiveApods,
                                       OverviewItemsType = _fiveApods.First().GetType(),
                                       OverviewItemsCsvFileName = $"spaceflight-apods_{DateTime.UtcNow:yyyy-MM-dd}.csv",
                                       OverviewItemsCsvButtonText = "Download CSV",
                                       CaptionText = "SpaceFlightNewsApp.Apod",
                                       HeaderTemplateStr = (MarkupString)
                                                                          """
                                                                          <thead>
                                                                            <tr>
                                                                              <th>Date</th>
                                                                              <th>Media</th>
                                                                              <th>Title</th>
                                                                            </tr>
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
                                                                            <tr>
                                                                              <th>Anonymous User Id</th>
                                                                              <th>Uri</th>
                                                                              <th>Last Accessed</th>
                                                                            </tr>
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
                                                                            <tr>
                                                                              <th>Digest</th>
                                                                              <th>Last Accessed</th>
                                                                            </tr>
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
                                                                            <tr>
                                                                                <th>Id</th>
                                                                                <th>Last Accessed</th>
                                                                            </tr>
                                                                          </thead>
                                                                          """,
                                       RowTemplateBuildStr = BuildUserAgentRow,
                               }
        ]);
    }
}
