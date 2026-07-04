using Microsoft.Extensions.Diagnostics.HealthChecks;
using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.API.HealthChecks;

public sealed class CacheHealthCheck : IHealthCheck
{
    private readonly ICacheService _cache;

    public CacheHealthCheck(ICacheService cache)
    {
        _cache = cache;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        const string key = "health-cache-test";

        try
        {
            await _cache.SetAsync(
                key,
                "healthy",
                TimeSpan.FromSeconds(30));

            var value = await _cache.GetAsync<string>(key);

            await _cache.RemoveAsync(key);

            if (value == "healthy")
            {
                return HealthCheckResult.Healthy("Cache is available.");
            }

            return HealthCheckResult.Unhealthy(
                "Cache returned an unexpected value.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Cache is unavailable.",
                ex);
        }
    }
}