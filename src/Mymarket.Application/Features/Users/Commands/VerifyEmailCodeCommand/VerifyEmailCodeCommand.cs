using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Models;
using Mymarket.Domain.Constants;
using FluentValidation;
using Mymarket.Application.features.Users.Common.Helpers;
using Mymarket.Application.features.Users.Common.Models;

namespace Mymarket.Application.features.Users.Commands.VerifyEmailCodeCommand;

public record VerifyEmailCodeCommand(string Email, int Code) : IRequest<AuthDto>;

public class VerifyEmailCodeCommandHandler(IApplicationDbContext _context, ITokenProvider _tokenProvider) : IRequestHandler<VerifyEmailCodeCommand, AuthDto>
{
    public async Task<AuthDto> Handle(VerifyEmailCodeCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.VerificationCode
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.User!.Email == request.Email && x.CodeType == CodeType.EmailVerification, cancellationToken);

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

        user.EmailVerified = true;

        var userModel = new UserModel
        {
            Id = user.Id,
            Name = user.Firstname,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Password = user.PasswordHash,
            EmailVerified = user.EmailVerified,
        };

        var (accessToken, expiresAt) = _tokenProvider.CreateAccessToken(userModel);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        record.IsVerified = true;

        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthDto
        (
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            User: new UserDto
            (
                Id: user.Id,
                Name: user.Firstname,
                Lastname: user.LastName,
                Email: user.Email,
                EmailVerified: user.EmailVerified
            )
        );
    }
}