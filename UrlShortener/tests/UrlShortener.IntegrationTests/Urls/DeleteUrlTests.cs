using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Xunit;
using UrlShortener.Application.Common.Models;
using UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;
using UrlShortener.Application.Features.Urls.Queries.GetUrls;
using UrlShortener.Application.Common.Responses;
using UrlShortener.IntegrationTests.Authentication;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Urls;

public sealed class DeleteUrlTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public DeleteUrlTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Delete_existing_url_should_succeed()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://google.com",
                null,
                null));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/urls/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content
            .ReadFromJsonAsync<ApiResponse>();

        result.Should().NotBeNull();
        result!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Deleted_url_should_not_appear_in_list()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        var create = await _client.PostAsJsonAsync(
            "/api/urls",
            new CreateShortUrlCommand(
                "https://github.com",
                null,
                null));

        var created = await create.Content
            .ReadFromJsonAsync<CreateShortUrlResponse>();

        await _client.DeleteAsync(
            $"/api/urls/{created!.Id}");

        // Act
        var response = await _client.GetAsync("/api/urls");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.Content.ReadFromJsonAsync<
            PagedResponse<UrlResponse>>();

        result.Should().NotBeNull();
        result!.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task Delete_non_existing_url_should_return_not_found()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();
        await AuthenticationHelper.AuthenticateAsync(_client);

        // Act
        var response = await _client.DeleteAsync(
            $"/api/urls/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_without_authentication_should_return_401()
    {
        // Arrange
        await _factory.ResetDatabaseAsync();

        // Act
        var response = await _client.DeleteAsync(
            $"/api/urls/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}