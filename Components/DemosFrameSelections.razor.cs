using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components;

public partial class DemosFrameSelections
{
    [Parameter]
    public required List<FrameSelectionOption> DemosSelections { get; set; }

    [Parameter]
    public FrameSelectionOption? SelectedFrame { get; set; } = null;
}
