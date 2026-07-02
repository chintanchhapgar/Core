namespace UrlShortener.Application.Features.Urls.Analytics;

public sealed class UrlAnalyticsResponse
{
    public Guid UrlId { get; init; }

    public string OriginalUrl { get; init; } = default!;

    public string ShortCode { get; init; } = default!;

    public int TotalClicks { get; init; }

    public IReadOnlyList<BrowserStatResponse> Browsers { get; init; }
        = [];

    public IReadOnlyList<OperatingSystemStatResponse> OperatingSystems { get; init; }
        = [];

    public IReadOnlyList<VisitResponse> RecentVisits { get; init; }
        = [];
}