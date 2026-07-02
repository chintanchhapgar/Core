namespace UrlShortener.Application.Abstractions.Services;

public sealed class RequestInfo
{
    public string? IpAddress { get; init; }

    public string? UserAgent { get; init; }

    public string? Browser { get; init; }

    public string? OperatingSystem { get; init; }

    public string? Referrer { get; init; }
}