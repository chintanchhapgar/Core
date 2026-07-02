using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Features.Users.DTOs;

namespace UrlShortener.Application.Features.Users.Commands.LoginUser;

public sealed record LoginUserCommand(
    string Email,
    string Password)
    : ICommand<LoginResponse>;