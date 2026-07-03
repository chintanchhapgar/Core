using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.GetUrls;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Abstractions.Persistence;

public interface IShortUrlRepository
{
    Task<ShortUrl?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShortUrl>> GetUrlsByUserIdAsync(
        Guid? userId,
        CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetByIdAndUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetWithVisitsAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddVisitAsync(
        ShortUrlVisit visit,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        ShortUrl entity,
        CancellationToken cancellationToken = default);

    void Update(ShortUrl entity);

    void Remove(ShortUrl entity);

    Task<int> CountAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountActiveAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountExpiredAsync(
        CancellationToken cancellationToken = default);

    Task<int> TotalClicksAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShortUrl>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetAccessibleUrlAsync(
        Guid id,
        bool isAdmin,
        Guid? userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ShortUrl>> GetAccessibleUrlsAsync(
        bool isAdmin,
        Guid? userId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<UrlResponse>> GetPagedAccessibleUrlsAsync(
    GetUrlsQuery request,
    bool isAdmin,
    Guid? userId,
    CancellationToken cancellationToken = default);
}