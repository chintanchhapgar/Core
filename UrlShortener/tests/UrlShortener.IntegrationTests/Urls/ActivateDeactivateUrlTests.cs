using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class ActivateDeactivateUrlTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ActivateDeactivateUrlTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Deactivate_url_should_set_is_active_false()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        // Act
        var deactivateResponse = await _client.PutAsync(
            $"/api/urls/{created!.Id}/deactivate",
            null);

        // Assert
        deactivateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await _client.GetFromJsonAsync<
            PagedResponse<UrlResponse>>("/api/urls");

        list.Should().NotBeNull();
        list!.Items.Single().IsActive.Should().BeFalse();
    }

    [Fact]
    public async Task Activate_url_should_set_is_active_true()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var createResponse = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        var created = await createResponse.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        await _client.PutAsync(
            $"/api/urls/{created!.Id}/deactivate",
            null);

        // Act
        var activateResponse = await _client.PutAsync(
            $"/api/urls/{created.Id}/activate",
            null);

        // Assert
        activateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var list = await _client.GetFromJsonAsync<
            PagedResponse<UrlResponse>>("/api/urls");

        list.Should().NotBeNull();
        list!.Items.Single().IsActive.Should().BeTrue();
    }

    [Fact]
    public async Task Deactivate_unknown_url_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.PutAsync(
            $"/api/urls/{Guid.NewGuid()}/deactivate",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Activate_unknown_url_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.PutAsync(
            $"/api/urls/{Guid.NewGuid()}/activate",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Activate_requires_authentication()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        // Act
        var response = await _client.PutAsync(
            $"/api/urls/{Guid.NewGuid()}/activate",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Deactivate_requires_authentication()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        // Act
        var response = await _client.PutAsync(
            $"/api/urls/{Guid.NewGuid()}/deactivate",
            null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}