namespace UrlShortener.Application.Common.Models;

public sealed class FilterRule
{
    public string Property { get; init; } = string.Empty;

    public string Operator { get; init; } = "eq";

    public string? Value { get; init; }
}