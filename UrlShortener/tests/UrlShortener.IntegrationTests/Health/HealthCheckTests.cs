using System.Net;
using FluentAssertions;
using Xunit;
using UrlShortener.IntegrationTests.Common;

namespace UrlShortener.IntegrationTests.Health;

public sealed class HealthCheckTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthCheckTests(
        CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_endpoint_should_return_OK()
    {
        // Act
        var response = await _client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}