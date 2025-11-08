using MediatR;
using Mymarket.Application.Users.Common;

namespace Mymarket.Application.Users.Commands;

public record RegisterUserCommand(
    string Name, 
    string Lastname, 
    string Email, 
    string PhoneNumber, 
    string Password) : IRequest<AuthDto>;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthDto>
{
    public Task<AuthDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userDto = new UserDto(
            Id: 1,
            Name: request.Name,
            Lastname: request.Lastname,
            Email: request.Email
        );

        var authDto = new AuthDto(
            RefreshToken: "dummy-token",
            ExpiresAt: DateTime.UtcNow.AddMinutes(60).ToString("O"),
            User: userDto
        );

        return Task.FromResult(authDto);
    }
}