using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;

namespace UrlShortener.Application.Features.Admin.Users.LockUser;

public sealed class LockUserCommandHandler
    : ICommandHandler<LockUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LockUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        LockUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
            throw new KeyNotFoundException("User not found.");

        user.Lock();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}