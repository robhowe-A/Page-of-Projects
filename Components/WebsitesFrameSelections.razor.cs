using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components;

public partial class WebsitesFrameSelections
{
    [Parameter]
    public required List<FrameSelectionOption> WebsitesSelections { get; set; }
    
    [Parameter]
    public FrameSelectionOption? SelectedFrame { get; set; } = null;
}
