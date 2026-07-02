namespace UrlShortener.Application.Features.Urls.Analytics;

public sealed class OperatingSystemStatResponse
{
    public string OperatingSystem { get; init; } = default!;

    public int Count { get; init; }
}