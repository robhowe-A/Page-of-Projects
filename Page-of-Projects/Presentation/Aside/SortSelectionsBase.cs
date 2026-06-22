// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;
using ProjectsPage.Domain;

namespace ProjectsPage.Components.Secondary;

public abstract class SortSelectionsBase : ComponentBase
{
    private SortOrder SortOrderSelected { get; set; }

    [Parameter]
    public required string DeploymentAs { get; set; }

    [Parameter]
    public EventCallback<(SortOrder, string)> OnResortedListSelected { get; set; }

    [Parameter]
    public required List<FrameSelectionOption> DemosSelections { get; set; }

    protected async Task InvokeSortAsync(SortOrder sortOrder)
    {
        SortOrderSelected = sortOrder;
        await OnResortedListSelected.InvokeAsync((SortOrderSelected, DeploymentAs));
    }
};
