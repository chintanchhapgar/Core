namespace UrlShortener.Application.Features.Urls.GetUrls;

public sealed class UrlResponse
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public int ClickCount { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime? ExpiresOnUtc { get; init; }
}