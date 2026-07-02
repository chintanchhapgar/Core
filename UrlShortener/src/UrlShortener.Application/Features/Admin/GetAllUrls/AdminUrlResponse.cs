namespace UrlShortener.Application.Features.Admin.GetAllUrls;

public sealed class AdminUrlResponse
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = default!;

    public string ShortCode { get; init; } = default!;

    public bool IsActive { get; init; }

    public int ClickCount { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime? ExpiresOnUtc { get; init; }
}