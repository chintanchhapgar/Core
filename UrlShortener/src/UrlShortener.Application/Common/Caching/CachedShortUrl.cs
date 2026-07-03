namespace UrlShortener.Application.Common.Caching;

public sealed record CachedShortUrl
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public DateTime? ExpiresOnUtc { get; init; }
}