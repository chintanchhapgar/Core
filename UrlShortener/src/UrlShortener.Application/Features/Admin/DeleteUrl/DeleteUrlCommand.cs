using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.DeleteUrl;

public sealed record DeleteUrlCommand(Guid UrlId)
    : ICommand;