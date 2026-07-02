using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Features.Urls.DTOs;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed class CreateShortUrlCommandHandler
    : ICommandHandler<CreateShortUrlCommand, ShortUrlDto>
{
    private readonly IShortUrlRepository _repository;
    private readonly IShortCodeGenerator _generator;
    private readonly ICurrentUserService _currentUser;
    public CreateShortUrlCommandHandler(
    IShortUrlRepository repository,
    IShortCodeGenerator generator,
    ICurrentUserService currentUser)
    {
        _repository = repository;
        _generator = generator;
        _currentUser = currentUser;
    }
    public async Task<ShortUrlDto> Handle(
    CreateShortUrlCommand request,
    CancellationToken cancellationToken)
    {
        string shortCode;

        if (!string.IsNullOrWhiteSpace(request.CustomAlias))
        {
            var exists = await _repository.GetByShortCodeAsync(
                request.CustomAlias,
                cancellationToken);

            if (exists is not null)
            {
                throw new InvalidOperationException(
                    "Custom alias already exists.");
            }

            shortCode = request.CustomAlias;
        }
        else
        {
            shortCode = await _generator.GenerateAsync(cancellationToken);
        }

        var entity = new ShortUrl(
            request.OriginalUrl,
            shortCode,
            _currentUser.UserId);

        entity.SetExpiration(request.ExpiresOnUtc);

        await _repository.AddAsync(entity, cancellationToken);

        return new ShortUrlDto
        {
            Id = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ClickCount = entity.ClickCount,
            ExpiresOnUtc = entity.ExpiresOnUtc,
            IsActive = entity.IsActive
        };
    }
}