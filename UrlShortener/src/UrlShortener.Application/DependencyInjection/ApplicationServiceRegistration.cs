using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Application.Behaviors;

namespace UrlShortener.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(ApplicationServiceRegistration).Assembly);

        return services;
    }
}