using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UrlShortener.Domain.Entities;

namespace UrlShortener.Domain.Interfaces;

public interface IShortUrlRepository
{
    Task<ShortUrl?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<ShortUrl?> GetByShortCodeAsync(string shortCode, CancellationToken cancellationToken = default);

    Task AddAsync(ShortUrl entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(ShortUrl entity, CancellationToken cancellationToken = default);

}