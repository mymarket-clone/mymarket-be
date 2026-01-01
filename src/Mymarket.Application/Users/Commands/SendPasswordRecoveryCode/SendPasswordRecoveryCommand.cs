using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common.Exceptions;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Users.Commands.SendPasswordRecoveryCode;

public record SendPasswordRecoveryCommand(string Email) : IRequest<Unit>;

public class SendPasswordRecoveryCommandHandler(
    IApplicationDbContext _context,
    IEmailSender _emailSender) : IRequestHandler<SendPasswordRecoveryCommand, Unit>
{
    public async Task<Unit> Handle(SendPasswordRecoveryCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower().Equals(request.Email.ToLower()),
                cancellationToken
            );

        if (user is not null)
        {
            if (!user.EmailVerified)
            {
                throw new EmailNotVerifiedException(user.Email);
            }

            try
            {
                var verificationCode = CryptoHelper.CreateVerificationCode();
                var hashedVerificationCode = CryptoHelper.HashVerificationCode(verificationCode);

                var existingRecord = await _context.VerificationCode
                    .FirstOrDefaultAsync(
                        x => x.UserId.Equals(user.Id) && x.CodeType == CodeType.PasswordRecovery,
                        cancellationToken
                    );

                if (existingRecord is not null)
                {
                    _context.VerificationCode.Remove(existingRecord);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                _emailSender.SendEmail(
                    SenderName: "Mymarket",
                    SenderEmail: "noreply@mymarket.info",
                    ToName: user.Firstname,
                    ToEmail: request.Email,
                    Subject: "Password recovery verification code",
                    TextContent: verificationCode
                );

                var verificationCodeEntity = new VerificationCodeEntity
                {
                    UserId = user.Id,
                    CodeHash = hashedVerificationCode,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                    CodeType = CodeType.PasswordRecovery,
                    IsVerified = false,
                };

                await _context.VerificationCode.AddAsync(verificationCodeEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to send password recovery email", ex);
            }
        }

        return Unit.Value;
    }
}
