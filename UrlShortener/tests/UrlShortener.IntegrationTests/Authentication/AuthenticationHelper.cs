using FluentAssertions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UrlShortener.Application.Features.Users.Commands.LoginUser;
using UrlShortener.Application.Features.Users.Commands.RegisterUser;
using UrlShortener.Application.Features.Users.DTOs;

namespace UrlShortener.IntegrationTests.Authentication;

public static class AuthenticationHelper
{
    public static async Task AuthenticateAsync(
        HttpClient client,
        string email = "admin@test.com",
        string password = "Password123!")
    {
        var register = new RegisterUserCommand(
        "Admin",
        "User",
        email,
        password);

        await client.PostAsJsonAsync("/api/auth/register", register);

        var login = new LoginUserCommand(
            email,
            password);

        var response = await client.PostAsJsonAsync(
            "/api/auth/login",
            login);

        response.EnsureSuccessStatusCode();

        var result =
            await response.Content.ReadFromJsonAsync<LoginResponse>();

        result.Should().NotBeNull();

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                result!.AccessToken);
    }
}