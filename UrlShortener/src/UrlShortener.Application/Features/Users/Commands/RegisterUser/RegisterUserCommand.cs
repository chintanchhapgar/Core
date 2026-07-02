using UrlShortener.Application.Abstractions.Messaging;

namespace UrlShortener.Application.Features.Users.Commands.RegisterUser;

public sealed record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password)
    : ICommand<Guid>;