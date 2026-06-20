// --Copyright (c) 2026 Robert A. Howell
using ProjectsPage.Domain;

namespace ProjectsPage.Infrastructure;

internal static class BackgroundHttpRequest
{
    internal static async Task LimitConcurrencyAndCheckUrls(Func<string, string, Task>? onResultAsync, List<string> urlsToCheck, Dictionary<string, string> statuses)
    {
        // Optional: limit concurrency so you don’t hammer the server/network
        using var throttler = new SemaphoreSlim(6);

        var tasks = urlsToCheck.Select(async url =>
                                       {
                                           await throttler.WaitAsync();

                                           try
                                           {
                                               var status = await HealthCheck.CheckGetRequestSuccessStatusCode(url);
                                               statuses[url] = status;

                                               if (onResultAsync is not null) await onResultAsync(url, status);
                                           }
                                           finally
                                           {
                                               throttler.Release();
                                           }
                                       });

        await Task.WhenAll(tasks);
    }

    internal static async Task<bool> SendHttpGetRequest(string url, string userAgent)
    {
        try
        {
            HttpClient http = new();

            http.DefaultRequestHeaders.Add("User-Agent", userAgent);

            using var response = await http.GetAsync(url);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }
};
