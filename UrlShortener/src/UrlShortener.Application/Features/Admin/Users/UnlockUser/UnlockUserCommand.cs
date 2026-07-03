using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.UnlockUser;

public sealed record UnlockUserCommand(Guid UserId)
    : ICommand;