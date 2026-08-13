// --Copyright (c) 2026 Robert A. Howell

using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Domain;

public abstract class FoldingDataTable : ComponentBase
{
    protected bool IsViewSummary { get; set; }
    private HashSet<string> _expanded { get; set; } = new();

    protected bool IsExpanded(string key) => _expanded.Contains(key);

    protected void Toggle(string key)
    {
        if (!_expanded.Remove(key))
        {
            _expanded.Add(key);
        }
    }

    protected void ToggleViewSummary()
    {
        IsViewSummary = !IsViewSummary;
    }

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        _expanded.Add(DateTime.Now.Year.ToString());
        _expanded.Add(DateTime.Now.Year + "-" + DateTime.Now.Month);
        _expanded.Add(DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day);
    }
};
