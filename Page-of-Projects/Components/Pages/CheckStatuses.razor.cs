using System.Text.Json;
using Microsoft.AspNetCore.Components;
using ProjectsPage.Components.Primary;
using ProjectsPage.Models;

namespace ProjectsPage.Components.Pages;

public class DomainOption
{
    public required string Tld { get; set; }
    public required List<SiteOption> Sites { get; init; } = new();
}

public class SiteOption
{
    public required string Name { get; set; }
    public required string Url { get; set; }
}

public partial class CheckStatuses : ComponentBase
{
    private const string Ok = "OK";
    private const string Down = "DOWN";

    private readonly List<DomainOption> _domainOptions = new FrameSelection().DomainOptionsData(Projects.Domains);
    private List<string> UrlsToCheck { get; set; } = new();

    private Task? _backgroundWork;
    
    private async Task RunChecksAsync()
    {
        // Optional: limit concurrency so you don’t hammer the server/network
        using var throttler = new SemaphoreSlim(initialCount: 6);

        var tasks = UrlsToCheck.Select(async url =>
        {
            await throttler.WaitAsync();
            try
            {
                var status = await CheckSiteStatus(url);
                _statuses[url] = status;

                // Trigger a re-render as each result arrives
                await InvokeAsync(StateHasChanged);
            }
            finally
            {
                throttler.Release();
            }
        });

        await Task.WhenAll(tasks);
    }
    
    public async Task<string> CheckSiteStatus(string url)
    {
        if(string.IsNullOrWhiteSpace(url)) throw new ArgumentNullException(nameof(url));
        
        return await IsSiteUp(url) ? Ok : Down;
    } 
    
    [Inject] private HttpClient Http { get; set; } = default!;
    
    private async Task<bool> IsSiteUp(string url)
    {
        try
        {
            using var response = await Http.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    private static List<DomainOption> DomainOptionsData()
    {
        return JsonSerializer.Deserialize<List<DomainOption>>(@"{}", new JsonSerializerOptions{ PropertyNameCaseInsensitive = true }) ?? throw new ApplicationException("DomainOptions is null") ;
    }
}
