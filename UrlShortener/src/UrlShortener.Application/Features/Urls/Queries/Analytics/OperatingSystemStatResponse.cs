namespace UrlShortener.Application.Features.Urls.Queries.Analytics;

public sealed class OperatingSystemStatResponse
{
    public string OperatingSystem { get; init; } = default!;

    public int Count { get; init; }
}