using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.DeactivateUser;

public sealed record DeactivateUserCommand(Guid UserId)
    : ICommand;