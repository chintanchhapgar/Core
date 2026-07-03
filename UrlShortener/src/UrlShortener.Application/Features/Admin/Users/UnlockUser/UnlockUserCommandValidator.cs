using FluentValidation;

namespace UrlShortener.Application.Features.Admin.Users.UnlockUser;

public sealed class UnlockUserCommandValidator
    : AbstractValidator<UnlockUserCommand>
{
    public UnlockUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}