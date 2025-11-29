using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Users.Commands.RegisterUser;

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

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage(SharedResources.GenderRequired);

        RuleFor(x => x.BirthYear)
            .NotEmpty().WithMessage(SharedResources.BirthDateRequired)
            .InclusiveBetween(1900, DateTime.UtcNow.Year - 16)
            .WithMessage(SharedResources.InvalidBirthYear);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .MinimumLength(8).WithMessage(SharedResources.PasswordMinLength)
            .MaximumLength(64).WithMessage(SharedResources.PasswordMaxLength)
            .Matches(@"^\S+$").WithMessage(SharedResources.PasswordNoSpaces)
            .Matches(@"[A-Z]").WithMessage(SharedResources.PasswordUppercase)
            .Matches(@"\d").WithMessage(SharedResources.PasswordNumber)
            .Matches(@"[\W_]").WithMessage(SharedResources.PasswordSpecial);

        RuleFor(x => x.PasswordConfirm)
            .NotEmpty().WithMessage(SharedResources.PasswordRequired)
            .Equal(x => x.Password).WithMessage(SharedResources.PasswordDoesnotMatch);
    }

    private async Task<bool> EmailAlreadyExist(string email, CancellationToken cancellationToken)
    {
        return !await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }
}
