// --Copyright (c) 2026 Robert A. Howell

using Microsoft.Playwright;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;
using System.Text.Json;
using Microsoft.Playwright.Xunit;

namespace UnitTests;

public class SortButtonTests : PageTest
{
    [Fact]
    public async Task SortButtonToggles_WhenClicked()
    {
        var SortButtonExpectedResults = new List<(string,string,string, string)>
                {   //(ButtonSelector, ExpectedLabel, ExpectedLabelChange, SortedSelector)
                    new (".details-panel-sort.sortAz","A-Z (Title)", "Z-A (Title)", ".page-view-link"),  // The first sort button should say "A-Z", switch to Z-A
                    new (".details-panel-sort.sortDate","Oldest", "Newest", ".created-date"),  // Second, oldest-to-newest
                    new (".details-panel-sort.sortLangAz","A-Z (Lang)","Z-A (Lang)", ".programming-lang")  // Third, programming language sort
                };

    await Page.GotoAsync(GlobalTests.Localhost);

        // Select "All" view
        await Page.GetByLabel("All").ClickAsync();

        foreach (var test in SortButtonExpectedResults)
        {
            string selector = test.Item4;

            // Select the sort button, check the text
            var sortButton = Page.Locator(test.Item1).First;
            await Expect(sortButton).ToHaveTextAsync(test.Item2);

            // Click switches the text and the sort order
            await sortButton.ClickAsync();
            await Expect(sortButton).ToHaveTextAsync(test.Item3);
            await CheckElementsSorted(selector, true);

            // Click again switches back the text and the sort order
            await sortButton.ClickAsync();
            await Expect(sortButton).ToHaveTextAsync(test.Item2);
            await CheckElementsSorted(selector, false);
        }

        async Task CheckElementsSorted(string selector, bool ascending)
        { // Collect all displayed item titles
            var titleElements = Page.Locator(selector);
            var count = await titleElements.CountAsync();
            Assert.True(count > 0, "Expected at least one item in the list.");

            var titles = new List<string>();

            for (int i = 0; i < count; i++)
            {
                titles.Add((await titleElements.Nth(i).InnerTextAsync()).Trim());
            }

            // Assert the list is sorted A-Z
            List<string> sortedTitles;

            if(ascending)
                sortedTitles = titles.OrderBy(t => t, StringComparer.OrdinalIgnoreCase).ToList();
            else
                sortedTitles = titles.OrderByDescending(t => t, StringComparer.OrdinalIgnoreCase).ToList();

            Assert.Equal(sortedTitles, titles);
        }
    }
}

internal static class GlobalTests
{
    public const string Localhost = "https://localhost:58442/";
}

public class EntityModelTests
{

    [Fact]
    public async Task LoadHomepageSuccess()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();

        var response = await page.GotoAsync(GlobalTests.Localhost);

        Assert.NotNull(response);
        Assert.True(response.Ok);
    }

    [Fact]
    public void JsonDocStoreConnectTest()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void JsonDocSoreCountConfirmation()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();
        var count = (from project in db.projects where project.Site == "ProjectsPage" select project.Site).Count();
        count += (from project in db.projects where project.Site == "ProjectsPageDemos" select project.Site).Count();
        db.Dispose();

        Assert.True(count == 11);
    }

    [Fact]
    public void CheckAgileStockNameMatchInDatase()
    {
        using ProjectsDbContext db = EntityModels.CreateProjectsDbContext();

        //var DocumentTitles = db.projects.FromSqlRaw("""select document->"$.Title" from projects where site='ProjectsPage';"""); //Not an OOM: no array or list method
        var agileStockDocument = (from project in db.projects where project.Site == "ProjectsPage" && project.Document.Contains("AgileStock Web") select project.Document).ToList();
        db.Dispose();

        if (agileStockDocument.Count < 1) throw new NullReferenceException();

        JsonSerializerOptions options = new JsonSerializerOptions(new JsonSerializerDefaults());
        options.PropertyNameCaseInsensitive = false;

        FrameSelectionOption jdocAgileStock = JsonSerializer.Deserialize<FrameSelectionOption>
            (agileStockDocument.First(), options)
            ?? throw new InvalidDataException("Encountered a document serialization error.");

        Assert.Equal("AgileStock Web", jdocAgileStock.Title.ToString());
    }
};
