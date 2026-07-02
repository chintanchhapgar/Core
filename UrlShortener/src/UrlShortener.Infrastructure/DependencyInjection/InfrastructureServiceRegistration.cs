using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Security;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Infrastructure.Authentication;
using UrlShortener.Infrastructure.Security;
using UrlShortener.Infrastructure.Services;

namespace UrlShortener.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IShortCodeGenerator, ShortCodeGenerator>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.Configure<JwtSettings>(
    configuration.GetSection(JwtSettings.SectionName));

        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}   