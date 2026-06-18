// --Copyright (c) 2026 Robert A. Howell

namespace ProjectsPage.Components.Primary;

public partial class PageFrame
{
    protected override void OnInitialized()
    {
        var selectedWebsite = GetPageOptionDetail();
        if (selectedWebsite != null) SelectedWebsite = selectedWebsite;
        if (SelectedWebsite != null) SelectedWebsiteChanged.InvokeAsync(SelectedWebsite);
    }
};
