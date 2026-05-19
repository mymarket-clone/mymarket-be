using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.Edit;

public record EditAdminUserCommand(
    int Id,
    string Firstname,
    string Lastname,
    string Email,
    GenderType Gender,
    int BirthYear,
    string PhoneNumber,
    bool EmailVerified
) : IRequest;

public class EditAdminUserCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditAdminUserCommand>
{
    public async Task Handle(EditAdminUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        await EnsureUniqueUserAsync(request.Email, request.PhoneNumber, request.Id, cancellationToken);

        user.Firstname = request.Firstname;
        user.LastName = request.Lastname;
        user.Email = request.Email;
        user.Gender = request.Gender;
        user.BirthYear = request.BirthYear;
        user.PhoneNumber = request.PhoneNumber;
        user.EmailVerified = request.EmailVerified;

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task EnsureUniqueUserAsync(string email, string phoneNumber, int userId, CancellationToken cancellationToken)
    {
        var existing = await context.Users
            .Where(x => x.Id != userId)
            .Where(x => x.Email == email || x.PhoneNumber == phoneNumber)
            .Select(x => new { x.Email, x.PhoneNumber })
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is null)
            return;

        var propertyName = existing.Email == email ? nameof(EditAdminUserCommand.Email) : nameof(EditAdminUserCommand.PhoneNumber);
        var message = existing.Email == email ? SharedResources.EmailAlreadyExists : SharedResources.PhoneNumberAlreadyExists;

        throw new ValidationException([new ValidationFailure(propertyName, message)]);
    }
}
