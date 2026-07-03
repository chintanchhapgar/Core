using FluentValidation;

namespace UrlShortener.Application.Features.Admin.Users.LockUser;

public sealed class LockUserCommandValidator
    : AbstractValidator<LockUserCommand>
{
    public LockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}