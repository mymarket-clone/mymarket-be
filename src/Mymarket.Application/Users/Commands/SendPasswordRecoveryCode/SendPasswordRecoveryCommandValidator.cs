using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Users.Commands.SendPasswordRecoveryCode;

public class SendPasswordRecoveryCommandValidator : AbstractValidator<SendPasswordRecoveryCommand>
{
    private readonly IApplicationDbContext _context;

    public SendPasswordRecoveryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MustAsync(EmailDoesNotExist).WithMessage(SharedResources.UserWithEmailDoesNotExist);
    }

    private async Task<bool> EmailDoesNotExist(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.AnyAsync(x => x.Email.Equals(email), cancellationToken);
    }
}
