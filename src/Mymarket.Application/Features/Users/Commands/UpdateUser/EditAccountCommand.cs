using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.UpdateUser;

public record EditAccountCommand(
    string Firstname,
    string Lastname,
    string Email,
    GenderType Gender,
    int BirthYear,
    string PhoneNumber
) : IRequest;

public class EditAccountCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<EditAccountCommand>
{
    public async Task Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = (int)currentUser.Id!;

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null) throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        user.Firstname = request.Firstname;
        user.LastName = request.Lastname;
        user.Email = request.Email;
        user.Gender = request.Gender;
        user.BirthYear = request.BirthYear;
        user.PhoneNumber = request.PhoneNumber;

        await context.SaveChangesAsync(cancellationToken);
    }
}