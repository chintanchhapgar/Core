using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Common.Extensions;

namespace UrlShortener.Application.Features.Urls.Commands.DeactivateUrl;

public sealed class DeactivateUrlCommandHandler
    : ICommandHandler<DeactivateUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUserService _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeactivateUrlCommandHandler(
       IShortUrlRepository repository,
        ICurrentUserService currentUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeactivateUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetRequiredAccessibleUrlAsync(
                        request.UrlId,
                        _currentUser.IsAdmin,
                        _currentUser.UserId,
                        cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("Short URL not found.");

        url.Deactivate();

        _repository.Update(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}