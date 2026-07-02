using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken);
}