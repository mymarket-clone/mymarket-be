using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Users.Commands.VerifyPasswodRecoveryCodeCommand;

public class VerifyPasswordRecoveryCodeCommandValidator : AbstractValidator<VerifyPasswordRecoveryCodeCommand>
{
    private readonly IApplicationDbContext _context;

    public VerifyPasswordRecoveryCodeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(SharedResources.EmailRequired)
            .EmailAddress().WithMessage(SharedResources.InvalidEmail)
            .MustAsync(UserExists).WithMessage(SharedResources.UserWithEmailDoesNotExist)
            .MustAsync(EmailIsVerified).WithMessage(SharedResources.EmailNotVerified);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(SharedResources.VerificationCodeRequired)
            .Length(6).WithMessage(SharedResources.CodeLength)
            .MustAsync(ValidRecoveryCode).WithMessage(SharedResources.CodeInvalidOrExpired);
    }

    private async Task<bool> UserExists(string email, CancellationToken ct)
    {
        return await _context.Users.AnyAsync(x => x.Email == email, ct);
    }

    private async Task<bool> EmailIsVerified(string email, CancellationToken ct)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email && x.EmailVerified, ct);
    }

    private async Task<bool> ValidRecoveryCode(VerifyPasswordRecoveryCodeCommand command, string code, CancellationToken ct)
    {
        var record = await _context.VerificationCode
            .Include(x => x.User)
            .FirstOrDefaultAsync(x =>
                x.User!.Email == command.Email &&
                x.CodeType == CodeType.PasswordRecovery,
                ct);

        if (record == null)
            return false;

        if (record.ExpiresAt < DateTime.UtcNow)
            return false;

        var inputHash = CryptoHelper.HashVerificationCode(code);

        return inputHash == record.CodeHash;
    }

}
