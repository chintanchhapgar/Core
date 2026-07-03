using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Admin.Users.UpdateRole;

public sealed record UpdateUserRoleCommand(
    Guid UserId,
    string Role)
    : ICommand;