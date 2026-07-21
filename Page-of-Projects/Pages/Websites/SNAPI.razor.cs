// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;
using ProjectsPage.Infrastructure;

namespace ProjectsPage.Components.Pages.Websites;

public partial class SNAPI
{
    private SpaceflightDbContext _articlesDbContext = EntityModels.CreateArticlesDbContext();
};
