using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.ActivateUser;

public sealed record ActivateUserCommand(Guid UserId)
    : ICommand;