// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public partial class SortSelectionsLang : SortSelectionsBase
{
    private async Task SortAllLangAz()
    {
        await InvokeSortAsync(SortOrder.LangAZ);
        _isSortedLangAz = true;
    }

    private async Task SortAllLangZa()
    {
        await InvokeSortAsync(SortOrder.LangZA);
        _isSortedLangAz = false;
    }
}