using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.features.Users.Commands.PasswordRecovery;

public record PasswordRecoveryCommand(string Code, string Password, string PasswordConfirm) : IRequest<Unit>;

public class PasswordRecoveryCommandHandler(IApplicationDbContext _context) : IRequestHandler<PasswordRecoveryCommand, Unit>
{
    public async Task<Unit> Handle(PasswordRecoveryCommand request, CancellationToken cancellationToken)
    {
        var codeHash = CryptoHelper.HashVerificationCode(request.Code.ToString());

        var user = await _context.VerificationCode
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.CodeHash == codeHash && x.CodeType == CodeType.PasswordRecovery,
                cancellationToken);

        if (user is not null)
        {
            user.User!.PasswordHash = CryptoHelper.HashPassword(request.Password);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}