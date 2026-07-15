// --Copyright (c) 2026 Robert A. Howell

using System.Text.Json;
using Microsoft.AspNetCore.Components;

namespace ProjectsPage.Components.Primary;

public class DomainOption
{
    public required string Tld { get; set; }
    public required List<SiteOption> Sites { get; init; } = new();
};

public class SiteOption
{
    public required string Name { get; set; }
    public required string Url { get; set; }
};

public partial class CheckStatuses : ComponentBase
{
    private List<string> UrlsToCheck { get; set; } = new();

    private static List<DomainOption> DomainOptionsData()
    {
        return JsonSerializer.Deserialize<List<DomainOption>>(@"{}",
                                                              new JsonSerializerOptions
                                                              { PropertyNameCaseInsensitive = true }) ??
                throw new ApplicationException("DomainOptions is null");
    }

    private async Task CheckDomainSitesRequestHealthAsync()
    {
        UrlsToCheck = _healthCheck.GetUrlsToCheck();
        
        await RunRequestHealthChecksAsync(UrlsToCheck);
    }

    private async Task RunRequestHealthChecksAsync(List<string>? urlsToCheck)
    {
        if (urlsToCheck != null)
        {
            foreach (var url in urlsToCheck)
                _statuses[url] = "…";
            await _healthCheck.RunChecksAsync(CheckStatusAsync, urlsToCheck);
        }

        await InvokeAsync(StateHasChanged);
    }

    private Func<string, string, Task> CheckStatusAsync
    {
        get
        {
            return async (url, status) =>
            {
               if (status != "OK")
                   _statuses[url] = "DOWN";
               else 
                   _statuses[url] = status;
               await InvokeAsync(StateHasChanged);
            };
        }
    }

};
