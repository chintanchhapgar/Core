using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using UrlShortener.Application.Common.Models;

namespace UrlShortener.Persistence.Extensions;

public static class FilterExtensions
{
    public static IQueryable<T> ApplyFilters<T>(
        this IQueryable<T> query,
        IEnumerable<FilterRule>? filters)
    {
        if (filters is null)
            return query;

        foreach (var filter in filters)
        {
            if (string.IsNullOrWhiteSpace(filter.Property))
                continue;

            var property = typeof(T).GetProperty(
                filter.Property,
                BindingFlags.IgnoreCase |
                BindingFlags.Public |
                BindingFlags.Instance);

            if (property is null)
                continue;

            var parameter = Expression.Parameter(typeof(T), "x");

            var member = Expression.Property(parameter, property);

            object? value = ConvertValue(
                property.PropertyType,
                filter.Value);

            if (value is null)
                continue;

            var constant = Expression.Constant(
                value,
                property.PropertyType);

            Expression? body = filter.Operator.ToLower() switch
            {
                "eq" => Expression.Equal(member, constant),

                "neq" => Expression.NotEqual(member, constant),

                "gt" => Expression.GreaterThan(member, constant),

                "gte" => Expression.GreaterThanOrEqual(member, constant),

                "lt" => Expression.LessThan(member, constant),

                "lte" => Expression.LessThanOrEqual(member, constant),

                _ => null
            };

            if (body is null)
                continue;

            var lambda = Expression.Lambda<Func<T, bool>>(
                body,
                parameter);

            query = query.Where(lambda);
        }

        return query;
    }

    private static object? ConvertValue(
        Type type,
        string? value)
    {
        if (value is null)
            return null;

        var actualType =
            Nullable.GetUnderlyingType(type) ?? type;

        return Convert.ChangeType(
            value,
            actualType,
            CultureInfo.InvariantCulture);
    }
}