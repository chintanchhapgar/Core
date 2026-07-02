using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Urls.Commands.DeleteUrl;

public sealed record DeleteUrlCommand(Guid UrlId)
    : ICommand;