using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IShortCodeGenerator, ShortCodeGenerator>();

        return services;
    }
}   