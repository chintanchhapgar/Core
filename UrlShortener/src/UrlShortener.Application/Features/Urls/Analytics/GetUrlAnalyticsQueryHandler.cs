using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Urls.Analytics;

public sealed class GetUrlAnalyticsQueryHandler
    : IQueryHandler<GetUrlAnalyticsQuery, UrlAnalyticsResponse>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUserService _currentUser;

    public GetUrlAnalyticsQueryHandler(
        IShortUrlRepository repository,
        ICurrentUserService currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<UrlAnalyticsResponse> Handle(
        GetUrlAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetWithVisitsAsync(
            request.UrlId,
            cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("URL not found.");

        if (url.UserId != _currentUser.UserId)
            throw new UnauthorizedAccessException();

        return new UrlAnalyticsResponse
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
    }
}