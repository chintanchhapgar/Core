using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using UrlShortener.Application.Abstractions.Authentication;

namespace UrlShortener.Infrastructure.Authentication;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(id, out var guid)
                ? guid
                : null;
        }
    }

    public bool IsAdmin =>
        _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
}