using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Name, 
    string Lastname, 
    string Email, 
    string Password,
    string PasswordConfirm) : IRequest<Unit>;

public class RegisterUserHandler(IApplicationDbContext _context) : IRequestHandler<RegisterUserCommand, Unit>
{
    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userToSave = new UserEntity
        {
            Name = request.Name,
            LastName = request.Lastname,
            Email = request.Email,
            EmailVerified = false,
            PasswordHash = CryptoHelper.HashPassword(request.Password),
        };

        _context.Users.Add(userToSave);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}