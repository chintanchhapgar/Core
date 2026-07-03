using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Admin.Users.GetUsers;
using UrlShortener.Domain.Entities;
using UrlShortener.Persistence.Context;
using UrlShortener.Persistence.Extensions;

namespace UrlShortener.Persistence.Repositories;

public sealed class UserRepository
    : RepositoryBase<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    : base(context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken)
    {
        return _context.Users.FirstOrDefaultAsync(
            x => x.Email == email,
            cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(
            user,
            cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public Task<List<ShortUrl>> GetUrlsAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return _context.ShortUrls
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedOnUtc)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.Users.CountAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(
            x => x.Id == id,
            cancellationToken);
    }

    public Task DeleteAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        _context.Users.Remove(user);

        return Task.CompletedTask;
    }

    public async Task<IReadOnlyList<UserWithUrlCount>> GetUsersWithUrlCountAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Select(u => new UserWithUrlCount
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Role = u.Role,
                UrlCount = _context.ShortUrls.Count(s => s.UserId == u.Id),
                IsActive = u.IsActive,
                IsLocked = u.IsLocked,
                CreatedOnUtc = u.CreatedOnUtc,
                UpdatedOnUtc = u.UpdatedOnUtc
            })
            .OrderBy(x => x.FirstName)
            .ThenBy(x => x.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResponse<UserResponse>> GetPagedAsync(
    GetUsersQuery request,
    CancellationToken cancellationToken = default)
    {
        var query = Query();

        query = query.Search(
            request.Search,
            x => x.FirstName,
            x => x.LastName,
            x => x.Email);

        query = query.ApplyFilters(request.Filters);

        query = query.ApplySorting(
            request.SortBy,
            request.Descending);

        if (string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = query.OrderByDescending(x => x.CreatedOnUtc);
        }

        return await query
            .Select(x => new UserResponse
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Role = x.Role,
                UrlCount = x.ShortUrls.Count,
                IsActive = x.IsActive,
                IsLocked = x.IsLocked,
                CreatedOnUtc = x.CreatedOnUtc,
                UpdatedOnUtc = x.UpdatedOnUtc
            })
            .ToPagedResponseAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);
    }
}