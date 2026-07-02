using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.DeactivateUrl;

public sealed record DeactivateUrlCommand(Guid UrlId)
    : ICommand;