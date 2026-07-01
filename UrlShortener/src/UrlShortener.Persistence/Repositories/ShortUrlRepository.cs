using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Persistence.Context;

namespace UrlShortener.Persistence.Repositories;

public sealed class ShortUrlRepository : IShortUrlRepository
{
    private readonly ApplicationDbContext _context;

    public ShortUrlRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        ShortUrl entity,
        CancellationToken cancellationToken = default)
    {
        await _context.ShortUrls.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        ShortUrl entity,
        CancellationToken cancellationToken = default)
    {
        _context.ShortUrls.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ShortUrl?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken = default)
    {
        return await _context.ShortUrls
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);
    }
}
