using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Abstractions.Security;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Infrastructure.Security;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IShortCodeGenerator, ShortCodeGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        return services;
    }
}   