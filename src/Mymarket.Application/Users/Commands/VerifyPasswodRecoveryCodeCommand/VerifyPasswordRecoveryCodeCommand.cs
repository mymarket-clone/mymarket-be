using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Users.Commands.VerifyPasswodRecoveryCodeCommand;

public record VerifyPasswordRecoveryCodeCommand(string Email, int Code) : IRequest<Unit>;

public class VerifyPasswordRecoveryCodeCommandHandler(IApplicationDbContext _context) : IRequestHandler<VerifyPasswordRecoveryCodeCommand, Unit>
{
    public async Task<Unit> Handle(VerifyPasswordRecoveryCodeCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.VerificationCode
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User!.Email == request.Email && x.CodeType == CodeType.PasswordRecovery, cancellationToken);

        if (record is null)
            throw new ValidationException(SharedResources.CodeNotFound);

        var isExpired = record.ExpiresAt < DateTime.UtcNow;

        if (isExpired)
            throw new ValidationException(SharedResources.CodeInvalidOrExpired);

        var inputCodeHash = CryptoHelper.HashVerificationCode(request.Code.ToString());

        if (inputCodeHash != record.CodeHash)
            throw new ValidationException(SharedResources.CodeInvalidOrExpired);

        var user = record.User
            ?? throw new ValidationException(SharedResources.UserWithEmailDoesNotExist);


        record.IsVerified = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
