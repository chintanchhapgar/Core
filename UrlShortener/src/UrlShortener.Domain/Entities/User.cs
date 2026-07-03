using UrlShortener.Domain.Common;
using UrlShortener.Domain.Constants;

namespace UrlShortener.Domain.Entities;

public sealed class User : AuditableEntity
{
    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Email { get; private set; } = default!;

    public string PasswordHash { get; private set; } = default!;

    public string Role { get; private set; } = Roles.User;

    public bool IsActive { get; private set; } = true;

    public bool IsLocked { get; private set; }

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
        string role = Roles.User)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;

        ChangeRole(role);

        IsLocked = false;
    }

    public void ChangeRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            throw new ArgumentException("Role is required.", nameof(role));

        if (role != Roles.Admin && role != Roles.User)
            throw new InvalidOperationException("Invalid role.");

        Role = role;
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }

    public void UpdateProfile(
        string firstName,
        string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void ChangePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }
}