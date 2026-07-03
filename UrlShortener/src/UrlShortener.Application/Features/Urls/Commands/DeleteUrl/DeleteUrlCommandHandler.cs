using UrlShortener.Application.Abstractions.Authentication;
using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Services;
using UrlShortener.Application.Common.Extensions;

namespace UrlShortener.Application.Features.Urls.Commands.DeleteUrl;

public sealed class DeleteUrlCommandHandler
    : ICommandHandler<DeleteUrlCommand>
{
    private readonly IShortUrlRepository _repository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;   

    public DeleteUrlCommandHandler(
        IShortUrlRepository repository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        DeleteUrlCommand request,
        CancellationToken cancellationToken)
    {
        var url = await _repository.GetRequiredAccessibleUrlAsync(
    request.UrlId,
    _currentUser.IsAdmin,
    _currentUser.UserId,
    cancellationToken);

        if (url is null)
            throw new KeyNotFoundException("Short URL not found.");


        _repository.Remove(url);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}