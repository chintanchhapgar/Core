namespace UrlShortener.Application.Features.Urls.Queries.Analytics;

public sealed class BrowserStatResponse
{
    public string Browser { get; init; } = default!;

    public int Count { get; init; }
}