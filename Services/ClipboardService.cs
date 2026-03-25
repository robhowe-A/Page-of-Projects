using Microsoft.JSInterop;
namespace ProjectsPage.Services;
public sealed class ClipboardService
{
    private readonly IJSRuntime _js;

    public ClipboardService(IJSRuntime js) => _js = js;

    public ValueTask CopyTextAsync(string? text)
        => string.IsNullOrWhiteSpace(text)
            ? ValueTask.CompletedTask
            : _js.InvokeVoidAsync("navigator.clipboard.writeText", text);
}