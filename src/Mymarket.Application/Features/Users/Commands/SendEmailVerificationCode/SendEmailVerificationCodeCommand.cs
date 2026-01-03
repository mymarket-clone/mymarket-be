using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.features.Users.Commands.SendEmailVerificationCode;

public record SendEmailVerificationCodeCommand(string Email) : IRequest<Unit>;

public class SendEmailVerificationCodeHandler(
    IApplicationDbContext _context, 
    IEmailSender _emailSender) : IRequestHandler<SendEmailVerificationCodeCommand, Unit>
{
    public async Task<Unit> Handle(SendEmailVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Email.ToLower().Equals(request.Email.ToLower()),
                cancellationToken
            );

        if (user is not null)
        {
            var verificationCode = CryptoHelper.CreateVerificationCode();
            var hashedVerificationCode = CryptoHelper.HashVerificationCode(verificationCode);

            try
            {
                var existingRecord = await _context.VerificationCode
                    .FirstOrDefaultAsync(
                        x => x.UserId.Equals(user.Id) && x.CodeType == CodeType.EmailVerification,
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
                    Subject: "Email verification code",
                    TextContent: verificationCode
                );

                var verificationCodeEntity = new VerificationCodeEntity
                {
                    UserId = user.Id,
                    CodeHash = hashedVerificationCode,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(3),
                    CodeType = CodeType.EmailVerification,
                    IsVerified = false
                };

                await _context.VerificationCode.AddAsync(verificationCodeEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to send email verification email", ex);
            }
        }

        return Unit.Value;
    }
}
