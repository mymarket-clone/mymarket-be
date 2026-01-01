using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Users.Commands.VerifyPasswodRecoveryCodeCommand;

public record VerifyPasswordRecoveryCodeCommand(string Email, string Code) : IRequest<Unit>;

public class VerifyPasswordRecoveryCodeCommandHandler : IRequestHandler<VerifyPasswordRecoveryCodeCommand, Unit>
{
    private readonly IApplicationDbContext _context;

    public VerifyPasswordRecoveryCodeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(
        VerifyPasswordRecoveryCodeCommand request,
        CancellationToken cancellationToken)
    {
        var record = await _context.VerificationCode
            .Include(x => x.User)
            .FirstAsync(x => x.User!.Email == request.Email && x.CodeType == CodeType.PasswordRecovery, cancellationToken);

        record.IsVerified = true;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
