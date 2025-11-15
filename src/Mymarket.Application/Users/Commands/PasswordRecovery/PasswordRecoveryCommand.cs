using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common.Helpers;

namespace Mymarket.Application.Users.Commands.PasswordRecovery;

public record PasswordRecoveryCommand(int Code, string Password, string PasswordConfirm) : IRequest<Unit>;

public class PasswordRecoveryCommandHandler(IApplicationDbContext _context) : IRequestHandler<PasswordRecoveryCommand, Unit>
{
    public async Task<Unit> Handle(PasswordRecoveryCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.VerificationCode
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User!.Id.Equals(x.UserId));

        if (user is not null)
        {
            user.User!.PasswordHash = CryptoHelper.HashPassword(request.Password);
            await _context.SaveChangesAsync(cancellationToken);
        }

        return Unit.Value;
    }
}