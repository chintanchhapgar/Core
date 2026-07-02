using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    CancellationToken cancellationToken);

    Task AddVisitAsync(
    ShortUrlVisit visit,
    CancellationToken cancellationToken);

    Task<ShortUrl?> GetWithVisitsAsync(
    Guid id,
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

    void Delete(ShortUrl shortUrl);
}
