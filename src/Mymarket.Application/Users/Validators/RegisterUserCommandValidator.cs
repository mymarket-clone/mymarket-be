using FluentValidation;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Commands;

namespace Mymarket.Application.Users.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.NameRequired)
            .MaximumLength(72).WithMessage(SharedResources.NameMaxLength);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage(SharedResources.LastNameRequired)
            .MaximumLength(72).WithMessage(SharedResources.LastNameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MaximumLength(256).WithMessage(SharedResources.EmailMaxLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(SharedResources.PhoneRequired)
            .MinimumLength(6).WithMessage(SharedResources.PhoneMinLength)
            .MaximumLength(18).WithMessage(SharedResources.PhoneMaxLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .MinimumLength(8).WithMessage(SharedResources.PasswordMinLength)
            .MaximumLength(64).WithMessage(SharedResources.PasswordMaxLength)
            .Matches(@"^\S+$").WithMessage(SharedResources.PasswordNoSpaces)
            .Matches(@"[A-Z]").WithMessage(SharedResources.PasswordUppercase)
            .Matches(@"\d").WithMessage(SharedResources.PasswordNumber)
            .Matches(@"[\W_]").WithMessage(SharedResources.PasswordSpecial);
    }
}
