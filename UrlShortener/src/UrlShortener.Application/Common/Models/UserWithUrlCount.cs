namespace UrlShortener.Application.Common.Models;

public sealed class UserWithUrlCount
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public int UrlCount { get; init; }
}