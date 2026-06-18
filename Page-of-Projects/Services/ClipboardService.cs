// --Copyright (c) 2026 Robert A. Howell

using Microsoft.JSInterop;

namespace ProjectsPage.Services;

public sealed class ClipboardService
{
    private readonly IJSRuntime _js;

    public ClipboardService(IJSRuntime js)
    {
        _js = js;
    }

    public ValueTask CopyTextAsync(string? text)
    {
        return string.IsNullOrWhiteSpace(text)
                ? ValueTask.CompletedTask
                : _js.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}