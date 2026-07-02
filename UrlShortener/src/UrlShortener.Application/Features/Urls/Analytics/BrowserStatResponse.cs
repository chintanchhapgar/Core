namespace UrlShortener.Application.Features.Urls.Analytics;

public sealed class BrowserStatResponse
{
    public string Browser { get; init; } = default!;

    public int Count { get; init; }
}