using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components;

public partial class FrameSelectionItems
{
    [Parameter]
    public required List<FrameSelectionOption> FrameSelections { get; set; }

    [Parameter]
    public FrameSelectionOption? SelectedFrame { get; set; } = null;
}
