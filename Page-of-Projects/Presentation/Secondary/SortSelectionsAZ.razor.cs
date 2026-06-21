// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class SortSelectionsAz : SortSelectionsBase
{
    private async Task SortDemosAz()
    {
        await InvokeSortAsync(SortOrder.AZ);
        _isSortedAz = true;
    }

    private async Task SortDemosZa()
    {
        await InvokeSortAsync(SortOrder.ZA);
        _isSortedAz = false;
    }
};
