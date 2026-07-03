using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.LockUser;

public sealed record LockUserCommand(Guid UserId)
    : ICommand;