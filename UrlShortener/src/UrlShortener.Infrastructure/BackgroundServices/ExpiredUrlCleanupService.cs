using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Infrastructure.Configuration;

namespace UrlShortener.Infrastructure.BackgroundServices;

public sealed class ExpiredUrlCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly CleanupJobOptions _options;
    private readonly ILogger<ExpiredUrlCleanupService> _logger;

    public ExpiredUrlCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<ExpiredUrlCleanupService> logger,
        IOptions<CleanupJobOptions> options)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "Expired URL cleanup service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation(
                    "Running expired URL cleanup...");

                using var scope = _scopeFactory.CreateScope();

                var repository =
                    scope.ServiceProvider.GetRequiredService<IShortUrlRepository>();

                var unitOfWork =
                    scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var cache =
                    scope.ServiceProvider.GetRequiredService<ICacheService>();

                var expiredUrls = await repository.GetExpiredActiveUrlsAsync(
                    stoppingToken);

                if (expiredUrls.Count == 0)
                {
                    _logger.LogInformation("No expired URLs found.");
                }
                else
                {
                    foreach (var url in expiredUrls)
                    {
                        url.Deactivate();

                        repository.Update(url);

                        await cache.RemoveAsync(CacheKeys.Url(url.ShortCode));
                        await cache.RemoveAsync(CacheKeys.UrlDetails(url.Id));
                        await cache.RemoveAsync(CacheKeys.Analytics(url.Id));
                    }

                    await unitOfWork.SaveChangesAsync(stoppingToken);

                    await cache.RemoveAsync(CacheKeys.Dashboard);

                    _logger.LogInformation(
                        "Deactivated {Count} expired URLs.",
                        expiredUrls.Count);
                }

                _logger.LogInformation(
                    "Cleanup completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error occurred during expired URL cleanup.");
            }

            await Task.Delay(
                TimeSpan.FromMinutes(_options.IntervalMinutes),
                stoppingToken);
        }
    }
}