using Serilog;
using Serilog.Context;
using UrlShortener.Application.Abstractions.Services;

namespace UrlShortener.API.Middleware;

public sealed class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext context,
        ICurrentUser currentUser)
    {        
        using (LogContext.PushProperty(
            "UserId",
            currentUser.UserId?.ToString() ?? "Anonymous"))
        using (LogContext.PushProperty(
            "IsAuthenticated",
            currentUser.IsAuthenticated))
        using (LogContext.PushProperty(
            "IsAdmin",
            currentUser.IsAdmin))
        {
            await _next(context);
        }

    }
}