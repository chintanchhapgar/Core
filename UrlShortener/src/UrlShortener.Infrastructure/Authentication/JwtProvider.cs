using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Features.Users.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Authentication;

public sealed class JwtProvider : IJwtProvider
{
    private readonly JwtSettings _settings;

    public JwtProvider(IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public LoginResponse Generate(User user)
    {
        var expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryMinutes);

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Email, user.Email),
        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_settings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        return new LoginResponse
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAtUtc = expires
        };
    }
}