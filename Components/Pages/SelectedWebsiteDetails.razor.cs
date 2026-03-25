using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components.Pages;

public partial class SelectedWebsiteDetails : ComponentBase
{
    [Parameter]
    public required FrameSelectionOption SelectedWebsite { get; set; }
    
    public void OnDetailsToggleOpen(EventArgs e)
    {
        ShowDetails = !ShowDetails;
    }
}