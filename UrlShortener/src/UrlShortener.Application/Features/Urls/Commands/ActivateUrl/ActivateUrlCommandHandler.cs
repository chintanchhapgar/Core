using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Extensions;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Urls.Commands.ActivateUrl;

public sealed class ActivateUrlCommandHandler
    : ICommandHandler<ActivateUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUrlCommandHandler(
        IShortUrlRepository repository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ActivateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetRequiredAccessibleUrlAsync(
     request.UrlId,
     _currentUser.IsAdmin,
     _currentUser.UserId,
     cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("Short URL not found.");

        url.Activate();

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}