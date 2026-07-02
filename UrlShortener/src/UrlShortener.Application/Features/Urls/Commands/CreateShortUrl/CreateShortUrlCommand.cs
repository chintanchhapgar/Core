using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Features.Urls.DTOs;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed record CreateShortUrlCommand(
    string OriginalUrl,
    string? CustomAlias,
    DateTime? ExpiresOnUtc)
    : ICommand<ShortUrlDto>;