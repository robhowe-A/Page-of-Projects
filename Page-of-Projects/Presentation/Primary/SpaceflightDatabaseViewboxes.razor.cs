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

    private string LimitWarning { get; set; } = string.Empty;
    private string ApodLimitWarning { get; set; } = string.Empty;
    private uint QueryLimit { get; set; } = 1000;

    private Task OnArticleFilterFindComplete((List<SpaceflightArticle> rows, bool isLimitReached) userQuery)
    {
        _articles = userQuery.rows.Take(Convert.ToInt16(QueryLimit)).ToArray(); // Limit placement
        LimitWarning = userQuery.isLimitReached ? $"Results view is limited to {QueryLimit} rows." : string.Empty;
        return Task.CompletedTask;
    }

    private Task OnApodFilterFindComplete((List<SpaceflightApod> rows, bool isLimitReached) userQuery)
    {
        _apods = userQuery.rows.Take(Convert.ToInt16(QueryLimit)).ToArray(); // Limit placement
        ApodLimitWarning = userQuery.isLimitReached ? $"Results view is limited to {QueryLimit} rows." : string.Empty;
        return Task.CompletedTask;
    }

    private bool? _isArticlePendingData = true;
    private bool? _isApodPendingData = true;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isArticlePendingData = true;
            _isApodPendingData = true;

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
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}