using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.Delete;

public record DeleteAdminUserCommand(int Id) : IRequest;

public class DeleteAdminUserCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<DeleteAdminUserCommand>
{
    public async Task Handle(DeleteAdminUserCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.Id == request.Id)
        {
            throw new ValidationException([new ValidationFailure(nameof(request.Id), "Cannot delete your own user.")]);
        }

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        if (user.AccessLevel == AccessLevelType.SuperAdmin)
        {
            var superAdminCount = await context.Users
                .CountAsync(x => x.AccessLevel == AccessLevelType.SuperAdmin, cancellationToken);

            if (superAdminCount <= 1)
            {
                throw new ValidationException([new ValidationFailure(nameof(request.Id), "Cannot delete the last SuperAdmin.")]);
            }
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
