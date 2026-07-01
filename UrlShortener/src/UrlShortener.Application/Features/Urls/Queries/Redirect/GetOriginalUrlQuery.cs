using UrlShortener.Application.Abstractions.Messaging;

public sealed record GetOriginalUrlQuery(string ShortCode)
    : IQuery<string?>;