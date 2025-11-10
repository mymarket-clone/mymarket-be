using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Mymarket.Application.Users.Commands.SendVerificationEmail;

public record SendVerificationEmailCommand(string Email) : IRequest<Unit>;

public class SendVeificationEmailHandler(
    IApplicationDbContext _context, 
    IEmailSender _emailSender) : IRequestHandler<SendVerificationEmailCommand, Unit>
{
    public async Task<Unit> Handle(SendVerificationEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email.Equals(request.Email), cancellationToken);

        if (user is not null)
        {
            var verificationCode = CryptoHelper.CreateVerificationCode();
            var hashedVerificationCode = CryptoHelper.HashVerificationCode(verificationCode);

            try
            {
                _emailSender.SendEmail(
                    SenderName: "Mymarket",
                    SenderEmail: "noreply@mymarket.info",
                    ToName: user.Name,
                    ToEmail: request.Email,
                    Subject: "Verification Code",
                    TextContent: verificationCode
                );

                var emailEntity = new EmailVerificationEntity
                {
                    UserId = user.Id,
                    CodeHash = hashedVerificationCode,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(5)
                };

                _context.EmailVerification.Add(emailEntity);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to send verification email", ex);
            }
        }

        return await Task.FromResult(Unit.Value);
    }
}
