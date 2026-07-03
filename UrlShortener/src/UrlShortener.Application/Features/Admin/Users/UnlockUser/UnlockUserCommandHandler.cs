using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.Users.UnlockUser;

public sealed class UnlockUserCommandHandler
    : ICommandHandler<UnlockUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UnlockUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        UnlockUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        user.Unlock();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}