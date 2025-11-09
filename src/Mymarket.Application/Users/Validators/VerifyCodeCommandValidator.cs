using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Commands;

namespace Mymarket.Application.Users.Validators;

public class VerifyCodeCommandValidator : AbstractValidator<VerifyCodeCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyCodeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(SharedResources.VerificationCodeRequired);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MustAsync(EmailDoesNotExist).WithMessage(SharedResources.UserWithEmailDoesNotExist)
            .MustAsync(EmailNotVerified).WithMessage(SharedResources.EmailAlreadyVerified);
    }

    private async Task<bool> EmailDoesNotExist(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }

    private async Task<bool> EmailNotVerified(string email, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        return user != null && !user.EmailVerified;
    }
}
