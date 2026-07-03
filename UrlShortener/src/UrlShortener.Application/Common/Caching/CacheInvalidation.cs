using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.Application.Common.Caching;

public static class CacheInvalidation
{
    public static async Task InvalidateUrlAsync(
        ICacheService cache,
        Guid urlId,
        string shortCode,
        bool removeAnalytics = true)
    {
        await cache.RemoveAsync(CacheKeys.Url(shortCode));
        await cache.RemoveAsync(CacheKeys.UrlDetails(urlId));

        if (removeAnalytics)
        {
            await cache.RemoveAsync(CacheKeys.Analytics(urlId));
        }

        await cache.RemoveAsync(CacheKeys.Dashboard);
    }

    public static async Task InvalidateAnalyticsAsync(
        ICacheService cache,
        Guid urlId)
    {
        await cache.RemoveAsync(CacheKeys.Analytics(urlId));
        await cache.RemoveAsync(CacheKeys.Dashboard);
    }
}