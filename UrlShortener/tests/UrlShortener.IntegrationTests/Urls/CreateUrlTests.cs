using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.DTOs;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class CreateUrlTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateUrlTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Create_url_should_return_created_url()
    {
        // Arrange
        await AuthenticationHelper.AuthenticateAsync(_client);

        var command = new CreateShortUrlCommand(
            "https://google.com",
            null,
            null);

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/urls",
            command);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result =
            await response.Content.ReadFromJsonAsync<ShortUrlDto>();

        result.Should().NotBeNull();

        result!.OriginalUrl.Should().Be("https://google.com");

        result.ShortCode.Should().NotBeNullOrWhiteSpace();

        result.ClickCount.Should().Be(0);

        result.IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Created_url_should_be_returned_in_list()
    {
        await AuthenticationHelper.AuthenticateAsync(_client);

        var command = new CreateShortUrlCommand(
            "https://github.com",
            null,
            null);

        await _client.PostAsJsonAsync("/api/urls", command);

        var response = await _client.GetAsync("/api/urls");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();

        content.Should().Contain("github.com");
    }
}