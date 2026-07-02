namespace UrlShortener.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public DateTime CreatedOnUtc { get; private set; }

    public ICollection<ShortUrl> ShortUrls { get; private set; }
        = new List<ShortUrl>();

    private User()
    {
    }

    public User(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedOnUtc = DateTime.UtcNow;
    }
}