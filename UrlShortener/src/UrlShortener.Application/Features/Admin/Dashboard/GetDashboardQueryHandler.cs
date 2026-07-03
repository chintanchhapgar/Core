using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Caching;
using UrlShortener.Application.Features.Admin.Dashboard;

public sealed class GetDashboardQueryHandler
    : IQueryHandler<GetDashboardQuery, DashboardResponse>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICacheService _cache;

    public GetDashboardQueryHandler(
        IShortUrlRepository repository,
        ICacheService cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<DashboardResponse> Handle(
        GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var cached = await _cache.GetAsync<DashboardResponse>(
            CacheKeys.Dashboard);

        if (cached is not null)
            return cached;

        var response = new DashboardResponse
        {
            TotalUrls = await _repository.CountAsync(cancellationToken),
            ActiveUrls = await _repository.CountActiveAsync(cancellationToken),
            ExpiredUrls = await _repository.CountExpiredAsync(cancellationToken),
            TotalClicks = await _repository.TotalClicksAsync(cancellationToken)
        };

        await _cache.SetAsync(
            CacheKeys.Dashboard,
            response,
            TimeSpan.FromMinutes(5));

        return response;
    }
}