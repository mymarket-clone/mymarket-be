using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common;
using Mymarket.Domain.Models;

namespace Mymarket.Application.Users.Commands;

public record RegisterUserCommand(
    string Name, 
    string Lastname, 
    string Email, 
    string PhoneNumber, 
    string Password) : IRequest<AuthDto>;

public class RegisterUserHandler(ITokenProvider _tokenProvider) : IRequestHandler<RegisterUserCommand, AuthDto>
{
    public Task<AuthDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserModel
        {
            Name = request.Name,
            Lastname = request.Lastname,
            Email = request.Email,
            Password = request.Password,
            PhoneNumber = request.PhoneNumber,
            EmailVerified = false
        };

        var (accessToken, expiresAt) = _tokenProvider.CreateAccessToken(user);
        var refreshToken = _tokenProvider.CreateRefreshToken();

        var authDto = new AuthDto(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: expiresAt,
            User: new UserDto(
                Id: 1,
                Name: request.Name,
                Lastname: request.Lastname,
                Email: request.Email,
                EmailVerified: false
            )
        );

        return Task.FromResult(authDto);
    }
}