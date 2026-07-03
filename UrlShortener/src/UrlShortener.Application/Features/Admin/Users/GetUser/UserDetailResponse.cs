namespace UrlShortener.Application.Features.Admin.Users.GetUser;

public sealed class UserDetailResponse
{
    public Guid Id { get; init; }

    public string FirstName { get; init; } = string.Empty;

    public string LastName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public int UrlCount { get; init; }

    public IReadOnlyList<UserUrlResponse> Urls { get; init; }
        = new List<UserUrlResponse>();
}

public sealed class UserUrlResponse
{
    public Guid Id { get; init; }

    public string OriginalUrl { get; init; } = string.Empty;

    public string ShortCode { get; init; } = string.Empty;

    public int ClickCount { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedOnUtc { get; init; }

    public DateTime? ExpiresOnUtc { get; init; }
}