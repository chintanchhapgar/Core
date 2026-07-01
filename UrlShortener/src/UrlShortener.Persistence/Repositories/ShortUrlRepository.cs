using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Context;

namespace UrlShortener.Persistence.Repositories;

public sealed class ShortUrlRepository
    : RepositoryBase<ShortUrl>,
      IShortUrlRepository
{
    public ShortUrlRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.ShortCode == shortCode,
            cancellationToken);
    }
}