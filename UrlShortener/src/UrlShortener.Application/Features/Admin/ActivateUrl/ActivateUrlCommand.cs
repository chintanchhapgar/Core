using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.ActivateUrl;

public sealed record ActivateUrlCommand(Guid UrlId) : ICommand;