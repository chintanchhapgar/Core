using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Domain.Constants;

namespace UrlShortener.Infrastructure.Services;

public sealed class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var value = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id)
                ? id
                : null;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated
        ?? false;

    public bool IsAdmin =>
        _httpContextAccessor.HttpContext?.User?.IsInRole(Roles.Admin)
        ?? false;
}