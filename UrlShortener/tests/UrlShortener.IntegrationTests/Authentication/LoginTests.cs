using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.Application.Features.Users.Commands.LoginUser;
using UrlShortener.Application.Features.Users.Commands.RegisterUser;
using UrlShortener.IntegrationTests.Common;
using Xunit;

namespace UrlShortener.IntegrationTests.Authentication;

public sealed class LoginTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoginTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_should_return_token()
    {
        var register = new RegisterUserCommand
        (
            "John",
            "Doe",
            "john@test.com",
            "Password123!"
        );

        await _client.PostAsJsonAsync(
            "/api/auth/register",
            register);

        var login = new LoginUserCommand
        (
            register.Email,
            register.Password
        );

        var response = await _client.PostAsJsonAsync(
            "/api/auth/login",
            login);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content.ReadAsStringAsync();

        body.Should().Contain("accessToken");
    }
}