namespace UrlShortener.Application.Features.Users.DTOs;

public sealed class LoginResponse
{
    public string AccessToken { get; init; } = string.Empty;

    public DateTime ExpiresAtUtc { get; init; }
}