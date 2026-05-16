using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.features.Users.Commands.VerifyPasswodRecoveryCodeCommand;

public record VerifyPasswordRecoveryCodeCommand(
    string Email,
    string Code
) : IRequest;

public class VerifyPasswordRecoveryCodeCommandHandler(
    IApplicationDbContext context) : IRequestHandler<VerifyPasswordRecoveryCodeCommand>
{
    public async Task Handle(
        VerifyPasswordRecoveryCodeCommand request,
        CancellationToken cancellationToken)
    {
        var record = await context.VerificationCode
            .Include(x => x.User)
            .FirstAsync(x => x.User!.Email == request.Email && x.CodeType == CodeType.PasswordRecovery, cancellationToken);

        record.IsVerified = true;

        await context.SaveChangesAsync(cancellationToken);
    }
}
