using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Persistence.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Paginate<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<T> Search<T>(
        this IQueryable<T> query,
        string? search,
        params Expression<Func<T, string?>>[] selectors)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        search = search.Trim().ToLower();

        var parameter = Expression.Parameter(typeof(T), "x");

        Expression? body = null;

        foreach (var selector in selectors)
        {
            var property = Expression.Invoke(selector, parameter);

            var notNull = Expression.NotEqual(
                property,
                Expression.Constant(null));

            var toLower = Expression.Call(
                property,
                typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);

            var contains = Expression.Call(
                toLower,
                nameof(string.Contains),
                Type.EmptyTypes,
                Expression.Constant(search));

            var expression = Expression.AndAlso(notNull, contains);

            body = body == null
                ? expression
                : Expression.OrElse(body, expression);
        }

        if (body == null)
            return query;

        var lambda = Expression.Lambda<Func<T, bool>>(body, parameter);

        return query.Where(lambda);
    }

    public static async Task<PagedResponse<T>> ToPagedResponseAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var totalRecords = await query.CountAsync(cancellationToken);

        var items = await query
            .Paginate(pageNumber, pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResponse<T>
        {
            Items = items,
            Pagination = new PaginationMetadata
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)pageSize)
            }
        };
    }
}