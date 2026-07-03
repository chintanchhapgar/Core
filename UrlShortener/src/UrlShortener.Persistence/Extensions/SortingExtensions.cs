using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Persistence.Extensions;

public static class SortingExtensions
{
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        string? sortBy,
        bool descending)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var property = typeof(T).GetProperties()
            .FirstOrDefault(x =>
                x.Name.Equals(sortBy,
                    StringComparison.OrdinalIgnoreCase));

        if (property is null)
            return query;

        var parameter = Expression.Parameter(typeof(T), "x");

        var propertyAccess = Expression.Property(parameter, property);

        var lambda = Expression.Lambda(propertyAccess, parameter);

        var method = descending
            ? nameof(Queryable.OrderByDescending)
            : nameof(Queryable.OrderBy);

        var expression = Expression.Call(
            typeof(Queryable),
            method,
            new[] { typeof(T), property.PropertyType },
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(expression);
    }
}