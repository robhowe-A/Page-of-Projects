using System.Text.Json;
using Microsoft.AspNetCore.Components;

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

    private readonly List<DomainOption> _domainOptions = DomainOptionsData();
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

    private const string DomainOptions = """
                                            [
                                                { 
                                                    "tld": "rhdeveloping.com",
                                                    "sites": [
                                                        {
                                                            "name": "Creative Single Page",
                                                            "url": "https://creative-demo.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "RH Developing Docs",
                                                            "url": "https://docs.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "Easybank",
                                                            "url": "https://easybank.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "Payment Form",
                                                            "url": "https://encryptedpaymentinformation-demo.rhdeveloping.com/form"
                                                        },
                                                        {
                                                            "name": "Secure Password Generator",
                                                            "url": "https://passwordgenerator-demo.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "Products",
                                                            "url": "https://products.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "Typing Speed Test",
                                                            "url": "https://typingspeedtest-demo.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "Quizzes App",
                                                            "url": "https://quizzes-demo.rhdeveloping.com/"
                                                        },
                                                        {
                                                            "name": "SpaceFlight News App",
                                                            "url": "https://rh-snapi-site.rhdeveloping.com/"
                                                        }
                                                    ]
                                                },
                                                {
                                                    "tld": "randomwebbits.com",
                                                    "sites": [
                                                        {
                                                            "name": "Random Web Bits",
                                                            "url": "https://www.randomwebbits.com/"
                                                        }
                                                    ]
                                                },
                                                {
                                                    "tld": "httprequest.app",
                                                    "sites": [
                                                        {
                                                            "name": "Http Request",
                                                            "url": "https://www.httprequest.app/"
                                                        }
                                                    ]
                                                }
                                            ] 
                                        """;
    
    private static List<DomainOption> DomainOptionsData()
    {
        return JsonSerializer.Deserialize<List<DomainOption>>(DomainOptions, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true }) ?? throw new ApplicationException("DomainOptions is null") ;
    }
}