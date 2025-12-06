using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Users.Common.Helpers;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Name, 
    string Lastname, 
    string Email, 
    GenderType Gender, 
    int BirthYear,
    string PhoneNumber,
    string Password,
    string PasswordConfirm) : IRequest;

public class RegisterUserHandler(IApplicationDbContext _context) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userToSave = new UserEntity
        {
            Name = request.Name,
            LastName = request.Lastname,
            Email = request.Email,
            Gender = request.Gender,
            BirthYear = request.BirthYear,
            PhoneNumber = request.PhoneNumber,
            EmailVerified = false,
            PasswordHash = CryptoHelper.HashPassword(request.Password),
        };

        _context.Users.Add(userToSave);
        await _context.SaveChangesAsync(cancellationToken);
    }
}