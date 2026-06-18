// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class SortSelectionsAZ
{
    private async Task SortDemosAz()
    {
        DemosSelections.Sort((a, b) => a.Title.CompareTo(b.Title, StringComparison.OrdinalIgnoreCase));
        SortOrderSelected = SortOrder.AZ;
        _isSortedAz = true;
        await OnResortedListSelected.InvokeAsync((SortOrderSelected, DeploymentAs));
    }

    private async Task SortDemosZa()
    {
        DemosSelections.Sort((a, b) => b.Title.CompareTo(a.Title, StringComparison.OrdinalIgnoreCase));
        SortOrderSelected = SortOrder.ZA;
        _isSortedAz = false;
        await OnResortedListSelected.InvokeAsync((SortOrderSelected, DeploymentAs));
    }
};
