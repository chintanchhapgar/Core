using UrlShortener.Application.Abstractions.Messaging;
using UrlShortener.Application.Abstractions.Persistence;
using UrlShortener.Application.Abstractions.Security;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler
    : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByEmailAsync(
            request.Email,
            cancellationToken);

        if (existingUser is not null)
        {
            throw new InvalidOperationException(
                "A user with this email already exists.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.FirstName,
            request.LastName,
            request.Email,
            passwordHash);

        await _userRepository.AddAsync(
            user,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}