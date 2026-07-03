
using UrlShortener.Application.Features.Admin.Users.GetUsers;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Common.Models;

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

    Task<IReadOnlyList<User>> GetAllAsync(
    CancellationToken cancellationToken = default);

    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<UserWithUrlCount>> GetUsersWithUrlCountAsync(
        CancellationToken cancellationToken = default);

    void Update(User user);

    Task<PagedResponse<UserResponse>> GetPagedAsync(
    GetUsersQuery request,
    CancellationToken cancellationToken = default);
}