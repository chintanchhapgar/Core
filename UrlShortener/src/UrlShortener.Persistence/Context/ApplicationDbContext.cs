using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Domain.Common;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Persistence.Context;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

    public DbSet<ShortUrlVisit> ShortUrlVisits => Set<ShortUrlVisit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();

        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();

        return base.SaveChanges();
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.SetCreated(_currentUser.UserId);
            }

            if (entry.State == EntityState.Modified)
            {
                // Never overwrite created information
                entry.Property(x => x.CreatedOnUtc).IsModified = false;
                entry.Property(x => x.CreatedBy).IsModified = false;

                entry.Entity.SetUpdated(_currentUser.UserId);
            }
        }
    }
}