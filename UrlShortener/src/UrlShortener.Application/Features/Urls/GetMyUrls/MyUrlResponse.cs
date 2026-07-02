namespace UrlShortener.Application.Features.Urls.GetMyUrls;

public sealed class MyUrlResponse
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public int ClickCount { get; init; }

    public DateTime CreatedOnUtc { get; init; }
}