// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ProjectsPage.Domain;

namespace ProjectsPage.Components.Primary;

public partial class SpaceflightDatabaseViewboxes : ComponentBase
{
    private SpaceflightArticle[] _articles = [];
    private SpaceflightApod[] _apods = [];

    private bool _isArticleFilterSubmitting;
    private bool _isApodFilterSubmitting;

    private Task OnArticleFilterSubmittingChanged(bool isSubmitting)
    {
        _isArticleFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnApodFilterSubmittingChanged(bool isSubmitting)
    {
        _isApodFilterSubmitting = isSubmitting;
        return Task.CompletedTask;
    }

    private Task OnArticleFilterFindComplete(List<SpaceflightArticle> rows)
    {
        _articles = rows.ToArray();
        return Task.CompletedTask;
    }

    private Task OnApodFilterFindComplete(List<SpaceflightApod> rows)
    {
        _apods = rows.ToArray();
        return Task.CompletedTask;
    }

    private bool _isArticlePendingData = true;
    private bool _isApodPendingData = true;

    protected override async Task OnInitializedAsync()
    {
        await using var context = EntityModels.CreateArticlesDbContext();

        try
        {
            _articles = await context.Article.AsNoTracking()
                                     .Where(x => x.ArticleNum >= 1)
                                     .ToArrayAsync();

            _apods = await context.APOD.AsNoTracking()
                                  .Where(x => x.Id >= 1)
                                  .ToArrayAsync();
        }
        catch (Exception exception)
        {
            throw new ApplicationException(exception.Message);
        }
        finally
        {
            _isArticlePendingData = false;
            _isApodPendingData = false;
        }
    }
}