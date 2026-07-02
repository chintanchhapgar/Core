using UrlShortener.Application.Features.Users.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Abstractions.Authentication;

public interface IJwtProvider
{
    LoginResponse Generate(User user);
}