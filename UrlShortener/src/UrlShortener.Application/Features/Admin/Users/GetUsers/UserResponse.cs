namespace UrlShortener.Application.Features.Admin.Users.GetUsers;

public sealed class UserResponse
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Role { get; init; } = string.Empty;

    public int UrlCount { get; init; }

    public bool IsActive { get; init; }

    public bool IsLocked { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime? UpdatedOnUtc { get; init; }
}