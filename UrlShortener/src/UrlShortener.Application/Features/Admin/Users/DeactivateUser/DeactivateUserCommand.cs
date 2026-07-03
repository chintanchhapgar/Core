using FluentValidation;

namespace UrlShortener.Application.Features.Admin.Users.DeactivateUser;

public sealed class DeactivateUserCommandValidator
    : AbstractValidator<DeactivateUserCommand>
{
    public DeactivateUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}