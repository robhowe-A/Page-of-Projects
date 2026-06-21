// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class SortSelectionsDateStart : SortSelectionsBase
{
    private async Task SortAllDateStart()
    {
        await InvokeSortAsync(SortOrder.NEWEST);
        _isSortedNewest = true;
    }

    private async Task SortAllDateEnd()
    {
        await InvokeSortAsync(SortOrder.OLDEST);
        _isSortedNewest = false;
    }
};
