using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Commands;

namespace Mymarket.Application.Users.Validators;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    private readonly IApplicationDbContext _context;

    public RegisterUserCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.NameRequired)
            .MaximumLength(72).WithMessage(SharedResources.NameMaxLength)
            .Matches(@"^\p{L}+$").WithMessage(SharedResources.NameOnlyLetters);

        RuleFor(x => x.Lastname)
            .NotEmpty().WithMessage(SharedResources.LastnameRequired)
            .MaximumLength(72).WithMessage(SharedResources.LastnameMaxLength)
            .Matches(@"^\p{L}+$").WithMessage(SharedResources.LastnameOnlyLetters);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MaximumLength(256).WithMessage(SharedResources.EmailMaxLength)
            .MustAsync(EmailAlreadyExist).WithMessage(SharedResources.EmailAlreadyExists);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(SharedResources.PhoneRequired)
            .MinimumLength(6).WithMessage(SharedResources.PhoneMinLength)
            .MaximumLength(18).WithMessage(SharedResources.PhoneMaxLength)
            .MustAsync(PhoneAlreadyExists).WithMessage(SharedResources.PhoneNumberAlreadyExists);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .MinimumLength(8).WithMessage(SharedResources.PasswordMinLength)
            .MaximumLength(64).WithMessage(SharedResources.PasswordMaxLength)
            .Matches(@"^\S+$").WithMessage(SharedResources.PasswordNoSpaces)
            .Matches(@"[A-Z]").WithMessage(SharedResources.PasswordUppercase)
            .Matches(@"\d").WithMessage(SharedResources.PasswordNumber)
            .Matches(@"[\W_]").WithMessage(SharedResources.PasswordSpecial);
    }

    private async Task<bool> EmailAlreadyExist(string email, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }

    private async Task<bool> PhoneAlreadyExists(string phoneNumber, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(x => x.PhoneNumber.Equals(phoneNumber), cancellationToken);
    }
}
