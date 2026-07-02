using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<List<ShortUrl>> GetUrlsAsync(
    Guid userId,
    CancellationToken cancellationToken);

    Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken);

    Task<int> CountAsync(
    CancellationToken cancellationToken = default);
}