namespace UrlShortener.Application.Features.Urls.Queries.Analytics;

public sealed class VisitResponse
{
    public DateTime VisitedOnUtc { get; init; }

    public string? Browser { get; init; }

    public string? OperatingSystem { get; init; }

    public string? Referrer { get; init; }

    public string? IpAddress { get; init; }
}