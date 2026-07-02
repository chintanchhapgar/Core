using Microsoft.AspNetCore.Http;
using UrlShortener.Application.Abstractions.Services;

namespace UrlShortener.Infrastructure.Services;

public sealed class RequestInfoProvider : IRequestInfoProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RequestInfoProvider(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public RequestInfo GetCurrentRequest()
    {
        var request = _httpContextAccessor.HttpContext?.Request;

        var userAgent = request?.Headers.UserAgent.ToString();

        return new RequestInfo
        {
            IpAddress = _httpContextAccessor.HttpContext?
                .Connection
                .RemoteIpAddress?
                .ToString(),

            UserAgent = userAgent,

            Browser = GetBrowser(userAgent),

            OperatingSystem = GetOperatingSystem(userAgent),

            Referrer = request?.Headers.Referer.ToString()
        };
    }

    private static string? GetBrowser(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return null;

        if (userAgent.Contains("Edg"))
            return "Edge";

        if (userAgent.Contains("Chrome"))
            return "Chrome";

        if (userAgent.Contains("Firefox"))
            return "Firefox";

        if (userAgent.Contains("Safari") &&
            !userAgent.Contains("Chrome"))
            return "Safari";

        return "Other";
    }

    private static string? GetOperatingSystem(string? userAgent)
    {
        if (string.IsNullOrWhiteSpace(userAgent))
            return null;

        if (userAgent.Contains("Windows"))
            return "Windows";

        if (userAgent.Contains("Mac OS"))
            return "macOS";

        if (userAgent.Contains("Linux"))
            return "Linux";

        if (userAgent.Contains("Android"))
            return "Android";

        if (userAgent.Contains("iPhone") ||
            userAgent.Contains("iPad"))
            return "iOS";

        return "Other";
    }
}