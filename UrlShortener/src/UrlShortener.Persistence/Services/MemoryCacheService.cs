using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Application.Abstractions.Caching;

namespace UrlShortener.Persistence.Services;

public sealed class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T?> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T? value);

        return Task.FromResult(value);
    }

    public Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null)
    {
        _cache.Set(
            key,
            value,
            expiration ?? TimeSpan.FromMinutes(10));

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _cache.Remove(key);

        return Task.CompletedTask;
    }
}