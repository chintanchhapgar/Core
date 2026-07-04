using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UrlShortener.Application.Features.Users.Commands.RegisterUser;
using UrlShortener.IntegrationTests.Common;
using Xunit;

namespace UrlShortener.IntegrationTests.Authentication;

public sealed class RegisterTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_should_create_user()
    {
        var command = new RegisterUserCommand
        (
            "Jane",
            "Doe",
            "jane@test.com",
            "Password123!"
        );

        var response = await _client.PostAsJsonAsync(
            "/api/auth/register",
            command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}