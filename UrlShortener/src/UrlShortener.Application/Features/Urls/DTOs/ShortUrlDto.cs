namespace UrlShortener.Application.Features.Urls.DTOs;

public sealed class ShortUrlDto
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = default!;

    public string ShortCode { get; init; } = default!;

    public string ShortUrl { get; init; } = default!;

    public int ClickCount { get; init; }

    public DateTime? ExpiresOnUtc { get; init; }

    public bool IsActive { get; init; }
}