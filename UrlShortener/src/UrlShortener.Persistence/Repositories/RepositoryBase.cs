using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrlShortener.Application.Common.Models;
using UrlShortener.Domain.Common;
using UrlShortener.Persistence.Common.Models;
using UrlShortener.Persistence.Context;

namespace UrlShortener.Persistence.Repositories;

public abstract class RepositoryBase<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext Context;

    protected readonly DbSet<TEntity> DbSet;

    protected RepositoryBase(ApplicationDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    protected IQueryable<TEntity> Query(bool tracking = false)
    {
        return tracking
            ? DbSet
            : DbSet.AsNoTracking();
    }

    protected async Task<PagedResponse<TResult>> GetPagedAsync<TResult>(
         IQueryable<TEntity> query,
        Expression<Func<TEntity, TResult>> selector,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(pageNumber, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var totalRecords = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(selector)
            .ToListAsync(cancellationToken);

        return new PagedResponse<TResult>
        {
            Items = items,
            Pagination = new PaginationMetadata
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            }
        };
    }
}