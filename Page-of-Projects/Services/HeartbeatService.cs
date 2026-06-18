// --Copyright (c) 2026 Robert A. Howell

using ProjectsPage.Infrastructure;

namespace ProjectsPage.Services;

internal class HeartbeatService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(TimeSpan.FromMinutes(10));
        WriteLine($"HeartbeatService started");
        
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await HeartbeatSend.Initialize(stoppingToken);
            }
            catch (Exception ex)
            {
                WriteLine($"HeartbeatService error: {ex.Message}");
            }
        }
    }
};
