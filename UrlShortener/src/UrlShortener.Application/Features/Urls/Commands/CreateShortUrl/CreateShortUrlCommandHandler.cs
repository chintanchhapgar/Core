using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using UrlShortener.Application.Features.Urls.DTOs;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.Application.Features.Urls.Commands.CreateShortUrl;

public sealed class CreateShortUrlCommandHandler
    : IRequestHandler<CreateShortUrlCommand, ShortUrlDto>
{
    private readonly IShortUrlRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateShortUrlCommandHandler(
        IShortUrlRepository repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ShortUrlDto> Handle(
        CreateShortUrlCommand request,
        CancellationToken cancellationToken)
    {
        var shortCode = Guid.NewGuid()
            .ToString("N")[..8];

        var entity = new ShortUrl(
            request.OriginalUrl,
            shortCode);

        await _repository.AddAsync(entity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new ShortUrlDto
        {
            Id = entity.Id,
            OriginalUrl = entity.OriginalUrl,
            ShortCode = entity.ShortCode,
            ClickCount = entity.ClickCount
        };
    }
}
