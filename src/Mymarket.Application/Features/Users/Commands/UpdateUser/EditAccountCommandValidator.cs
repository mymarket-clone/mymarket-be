using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Users.Commands.UpdateUser;

public class EditAccountCommandValidator : AbstractValidator<EditAccountCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public EditAccountCommandValidator(
        IApplicationDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;

        RuleFor(x => x.Firstname)
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
            .MustAsync(EmailUniqueIfChanged).WithMessage(SharedResources.EmailAlreadyExists);

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage(SharedResources.GenderRequired);

        RuleFor(x => x.BirthYear)
            .NotEmpty().WithMessage(SharedResources.BirthDateRequired);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage(SharedResources.PhoneRequired)
            .MustAsync(PhoneUniqueIfChanged).WithMessage(SharedResources.PhoneNumberAlreadyExists);
    }

    private async Task<bool> EmailUniqueIfChanged(string email, CancellationToken cancellationToken)
    {
        var userId = _currentUser.Id;

        return !await _context.Users
            .AnyAsync(x => x.Email == email && x.Id != userId, cancellationToken);
    }

    private async Task<bool> PhoneUniqueIfChanged(string phone, CancellationToken cancellationToken)
    {
        var userId = _currentUser.Id;

        return !await _context.Users
            .AnyAsync(x => x.PhoneNumber == phone && x.Id != userId, cancellationToken);
    }
}