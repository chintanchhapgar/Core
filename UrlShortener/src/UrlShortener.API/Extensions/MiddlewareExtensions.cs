using UrlShortener.API.Middleware;

namespace UrlShortener.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }

    public static IApplicationBuilder UseUserContext(
        this IApplicationBuilder app)
    {
        return app.UseMiddleware<UserContextMiddleware>();
    }
}