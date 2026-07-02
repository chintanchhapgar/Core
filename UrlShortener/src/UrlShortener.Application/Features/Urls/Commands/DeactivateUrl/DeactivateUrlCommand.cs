using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Commands.DeactivateUrl;

public sealed record DeactivateUrlCommand(Guid UrlId)
    : ICommand;