// --Copyright (c) 2026 Robert A. Howell

using Microsoft.Playwright;
using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;
using System.Text.Json;

namespace UnitTests;

public class EntityModelTests
{
    [Fact]
    public async Task LoadHomepageSuccess()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync();
        var page = await browser.NewPageAsync();

        var response = await page.GotoAsync("https://localhost:58442/");

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

        Assert.True(count == 12);
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
