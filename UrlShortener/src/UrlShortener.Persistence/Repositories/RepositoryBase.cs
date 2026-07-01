using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Common;
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

    public virtual async Task AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
    }

    public virtual void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public virtual void Remove(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public virtual async Task<TEntity?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }
}