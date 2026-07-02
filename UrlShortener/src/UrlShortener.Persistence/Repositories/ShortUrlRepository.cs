using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Context;

namespace UrlShortener.Persistence.Repositories;

public sealed class ShortUrlRepository
    : RepositoryBase<ShortUrl>,
      IShortUrlRepository
{
    private readonly ApplicationDbContext _context;

    public ShortUrlRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.ShortCode == shortCode,
            cancellationToken);
    }

    public async Task<IReadOnlyList<ShortUrl>> GetUrlsByUserIdAsync(
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetWithVisitsAsync(
    Guid id,
    CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddVisitAsync(
        ShortUrlVisit visit,
        CancellationToken cancellationToken = default)
    {
        await _context.ShortUrlVisits.AddAsync(
            visit,
            cancellationToken);
    }
    public Task<int> CountAsync(
    CancellationToken cancellationToken = default)
    {
        return DbSet.CountAsync(cancellationToken);
    }

    public Task<int> CountActiveAsync(
        CancellationToken cancellationToken = default)
    {
        return DbSet.CountAsync(
            x => x.IsActive,
            cancellationToken);
    }

    public Task<int> CountExpiredAsync(
        CancellationToken cancellationToken = default)
    {
        return DbSet.CountAsync(
            x => x.ExpiresOnUtc.HasValue &&
                 x.ExpiresOnUtc.Value < DateTime.UtcNow,
            cancellationToken);
    }

    public Task<int> TotalClicksAsync(
        CancellationToken cancellationToken = default)
    {
        return DbSet.SumAsync(
            x => x.ClickCount,
            cancellationToken);
    }

    public async Task<IReadOnlyList<ShortUrl>> GetAllAsync(
    CancellationToken cancellationToken = default)
    {
        return await DbSet
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public void Delete(ShortUrl shortUrl)
    {
        DbSet.Remove(shortUrl);
    }
}