namespace UrlShortener.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public DateTime CreatedOnUtc { get; private set; }

    public string Role { get; private set; } = "User";

    public ICollection<ShortUrl> ShortUrls { get; private set; }
        = new List<ShortUrl>();

    private User()
    {
    }

    public User(
    string firstName,
    string lastName,
    string email,
    string passwordHash,
    string role = "User")
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
    }
}