using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Application.Users.Common.Dto;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Mymarket.Application.Users.Commands;

public record VerifyCodeCommand(string Email, int Code) : IRequest<AuthDto>;

public class VerifyCodeCommandHandle(IApplicationDbContext _context, ITokenProvider _tokenProvider) : IRequestHandler<VerifyCodeCommand, AuthDto>
{
    public async Task<AuthDto> Handle(VerifyCodeCommand request, CancellationToken cancellationToken)
    {
        var record = await _context.EmailVerification
            .Include(x => x.User)
            .Where(x => x.User!.Email == request.Email)
            .FirstOrDefaultAsync(cancellationToken);

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
            Name = user.Name,
            Lastname = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Password = user.PasswordHash,
            EmailVerified = user.EmailVerified,
        };

        var (accessToken, expiresAt) = _tokenProvider.CreateAccessToken(userModel);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        user.RefreshToken = refreshToken;

        _context.EmailVerification.Remove(record);
        await _context.SaveChangesAsync(cancellationToken);

        return new AuthDto
        (
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            User: new UserDto
            (
                Id: user.Id,
                Name: user.Name,
                Lastname: user.LastName,
                Email: user.Email,
                EmailVerified: user.EmailVerified
            )
        );
    }
}