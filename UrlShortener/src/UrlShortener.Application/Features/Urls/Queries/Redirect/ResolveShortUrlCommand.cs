using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Commands.ResolveShortUrl;

public sealed record ResolveShortUrlCommand(string ShortCode)
    : ICommand<string?>;