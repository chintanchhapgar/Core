namespace UrlShortener.Domain.Entities;

public sealed class ShortUrlVisit
{
    public Guid Id { get; private set; }

    public Guid ShortUrlId { get; private set; }

    public DateTime VisitedOnUtc { get; private set; }

    public string? IpAddress { get; private set; }

    public string? UserAgent { get; private set; }

    public string? Browser { get; private set; }

    public string? OperatingSystem { get; private set; }

    public string? Referrer { get; private set; }

    private ShortUrlVisit()
    {
    }

    public ShortUrlVisit(
        Guid shortUrlId,
        string? ipAddress,
        string? userAgent,
        string? browser,
        string? operatingSystem,
        string? referrer)
    {
        Id = Guid.NewGuid();
        ShortUrlId = shortUrlId;
        VisitedOnUtc = DateTime.UtcNow;
        IpAddress = ipAddress;
        UserAgent = userAgent;
        Browser = browser;
        OperatingSystem = operatingSystem;
        Referrer = referrer;
    }
}