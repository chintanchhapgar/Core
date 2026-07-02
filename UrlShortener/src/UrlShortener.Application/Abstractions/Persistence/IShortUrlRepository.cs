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
}
