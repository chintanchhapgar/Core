using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.GetUrls;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Context;
using UrlShortener.Persistence.Extensions;

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

    public async Task<ShortUrl?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
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

    public async Task<ShortUrl?> GetByIdAndUserAsync(
        Guid id,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.Id == id &&
                 x.UserId == userId,
            cancellationToken);
    }

    public async Task<ShortUrl?> GetWithVisitsAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddVisitAsync(
        ShortUrlVisit visit,
        CancellationToken cancellationToken = default)
    {
        await _context.ShortUrlVisits.AddAsync(
            visit,
            cancellationToken);
    }

    public async Task AddAsync(
        ShortUrl entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(ShortUrl entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(ShortUrl entity)
    {
        DbSet.Remove(entity);
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
                 x.ExpiresOnUtc.Value <= DateTime.UtcNow,
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

    public async Task<ShortUrl?> GetAccessibleUrlAsync(
        Guid id,
        bool isAdmin,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ShortUrl> query = DbSet;

        if (!isAdmin)
        {
            query = query.Where(x => x.UserId == userId);
        }

        return await query
            .Include(x => x.Visits)
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ShortUrl>> GetAccessibleUrlsAsync(
        bool isAdmin,
        Guid? userId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<ShortUrl> query = DbSet;

        if (!isAdmin)
        {
            query = query.Where(x => x.UserId == userId);
        }

        return await query
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResponse<UrlResponse>> GetPagedAccessibleUrlsAsync(
    GetUrlsQuery request,
    bool isAdmin,
    Guid? userId,
    CancellationToken cancellationToken = default)
    {
        var query = Query();

        if (!isAdmin)
        {
            query = query.Where(x => x.UserId == userId);
        }

        query = query.Search(
            request.Search,
            x => x.OriginalUrl,
            x => x.ShortCode);

        query = query.ApplyFilters(request.Filters);

        query = query.ApplySorting(
            request.SortBy,
            request.Descending);

        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderByDescending(x => x.CreatedOnUtc);
        }

        return await query
            .Select(x => new UrlResponse
            {
                Id = x.Id,
                OriginalUrl = x.OriginalUrl,
                ShortCode = x.ShortCode,
                ClickCount = x.ClickCount,
                IsActive = x.IsActive,
                CreatedOnUtc = x.CreatedOnUtc,
                ExpiresOnUtc = x.ExpiresOnUtc
            })
            .ToPagedResponseAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);
    }

    public async Task<IReadOnlyList<ShortUrl>> GetExpiredActiveUrlsAsync(
    CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(x =>
                x.IsActive &&
                x.ExpiresOnUtc.HasValue &&
                x.ExpiresOnUtc <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);
    }
}