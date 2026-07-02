using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Commands.UpdateUrl;

public sealed record UpdateUrlCommand(
    Guid UrlId,
    string OriginalUrl,
    DateTime? ExpirationDateUtc)
    : ICommand;