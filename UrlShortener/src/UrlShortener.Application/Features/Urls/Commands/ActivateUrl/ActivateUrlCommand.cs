using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Commands.ActivateUrl;

public sealed record ActivateUrlCommand(Guid UrlId) : ICommand;