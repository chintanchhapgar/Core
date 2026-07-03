using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Caching;

namespace UrlShortener.Application.Features.Urls.Queries.Analytics;

public sealed class GetUrlAnalyticsQueryHandler
    : IQueryHandler<GetUrlAnalyticsQuery, UrlAnalyticsResponse>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly ICacheService _cache;

    public GetUrlAnalyticsQueryHandler(
        IShortUrlRepository repository,
        ICurrentUser currentUser,
        ICacheService cache)
    {
        _repository = repository;
        _currentUser = currentUser;
        _cache = cache;
    }

    public async Task<UrlAnalyticsResponse> Handle(
        GetUrlAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.Analytics(request.UrlId);

        var cached =
            await _cache.GetAsync<UrlAnalyticsResponse>(cacheKey);

        if (cached is not null)
        {
            return cached;
        }

        var url = await _repository.GetWithVisitsAsync(
            request.UrlId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("URL not found.");

        if (url.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException();

        var response = new UrlAnalyticsResponse
        {
            UrlId = url.Id,
            OriginalUrl = url.OriginalUrl,
            ShortCode = url.ShortCode,
            TotalClicks = url.ClickCount,

            Browsers = url.Visits
                .GroupBy(x => x.Browser)
                .Select(x => new BrowserStatResponse
                {
                    Browser = x.Key ?? "Unknown",
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList(),

            OperatingSystems = url.Visits
                .GroupBy(x => x.OperatingSystem)
                .Select(x => new OperatingSystemStatResponse
                {
                    OperatingSystem = x.Key ?? "Unknown",
                    Count = x.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList(),

            RecentVisits = url.Visits
                .OrderByDescending(x => x.VisitedOnUtc)
                .Take(20)
                .Select(x => new VisitResponse
                {
                    VisitedOnUtc = x.VisitedOnUtc,
                    Browser = x.Browser,
                    OperatingSystem = x.OperatingSystem,
                    Referrer = x.Referrer,
                    IpAddress = x.IpAddress
                })
                .ToList()
        };

        await _cache.SetAsync(
            cacheKey,
            response,
            TimeSpan.FromMinutes(5));

        return response;
    }
}