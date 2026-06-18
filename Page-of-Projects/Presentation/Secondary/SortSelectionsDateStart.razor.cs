// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class SortSelectionsDateStart
{
    private async Task SortAllDateStart()
    {
        SortOrderSelected = SortOrder.OLDEST;
        _isSortedNewest = false;
        await OnResortedListSelected.InvokeAsync((SortOrderSelected, DeploymentAs));
    }

    private async Task SortAllDateEnd()
    {
        SortOrderSelected = SortOrder.NEWEST;
        _isSortedNewest = true;
        await OnResortedListSelected.InvokeAsync((SortOrderSelected, DeploymentAs));
    }
};
