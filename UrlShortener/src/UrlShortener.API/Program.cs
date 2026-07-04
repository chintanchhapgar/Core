using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using UrlShortener.API.Exceptions;
using UrlShortener.API.Extensions;
using UrlShortener.API.HealthChecks;
using UrlShortener.API.Logging;
using UrlShortener.Application.Abstractions.Caching;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.DependencyInjection;
using UrlShortener.Infrastructure.Authentication;
using UrlShortener.Infrastructure.DependencyInjection;
using UrlShortener.Persistence.Context;
using UrlShortener.Persistence.DependencyInjection;
using UrlShortener.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddControllers();

builder.Services.AddRateLimiting();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddApplication();

builder.Services.AddPersistence(builder.Configuration);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddMemoryCache();

builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

builder.Services.AddHealthChecks()
    .AddCheck<DatabaseHealthCheck>("database")
    .AddCheck<MemoryHealthCheck>("memory")
    .AddCheck<CacheHealthCheck>("cache");

var jwt = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.SecretKey))
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Authentication is required to access this resource."
                });
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "You do not have permission to access this resource."
                });
            }
        };
    });

try
{
    Log.Information("Starting UrlShortener API");

    var app = builder.Build();

    Log.Information("UrlShortener API started successfully.");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseExceptionHandler();

    app.UseCorrelationId();

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseUserContext();

    app.UseSerilogRequestLogging(options =>
    {
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            var currentUser = httpContext.RequestServices
                .GetRequiredService<ICurrentUser>();

            diagnosticContext.Set(
                "UserId",
                currentUser.UserId?.ToString() ?? "Anonymous");

            diagnosticContext.Set(
                "IsAuthenticated",
                currentUser.IsAuthenticated);

            diagnosticContext.Set(
                "IsAdmin",
                currentUser.IsAdmin);

            diagnosticContext.Set(
                "CorrelationId",
                httpContext.Response.Headers["X-Correlation-ID"].ToString());

            diagnosticContext.Set(
                "RequestHost",
                httpContext.Request.Host.Value);

            diagnosticContext.Set(
                "RequestScheme",
                httpContext.Request.Scheme);
        };
    });

    if (!app.Environment.IsEnvironment("Testing"))
    {
        app.UseRateLimiter();
    }

    app.UseAuthorization();

    app.MapControllers();

    app.MapApplicationHealthChecks();

    Log.Information("Environment: {Environment}", app.Environment.EnvironmentName);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program
{
}