using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Users.Commands.SetSuperAdmin;

public record SetUserSuperAdminCommand(
    int Id,
    bool IsSuperAdmin
) : IRequest;

public class SetUserSuperAdminCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<SetUserSuperAdminCommand>
{
    public async Task Handle(SetUserSuperAdminCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsSuperAdmin && currentUser.Id == request.Id)
        {
            throw new ValidationException(
                [new ValidationFailure(nameof(request.IsSuperAdmin), "Cannot remove your own SuperAdmin access.")]);
        }

        var user = await context.Users
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        if (!request.IsSuperAdmin && user.AccessLevel == AccessLevelType.SuperAdmin)
        {
            var superAdminCount = await context.Users
                .CountAsync(x => x.AccessLevel == AccessLevelType.SuperAdmin, cancellationToken);

            if (superAdminCount <= 1)
            {
                throw new ValidationException(
                    [new ValidationFailure(nameof(request.IsSuperAdmin), "Cannot unset the last SuperAdmin.")]);
            }
        }

        user.AccessLevel = request.IsSuperAdmin
            ? AccessLevelType.SuperAdmin
            : AccessLevelType.User;

        await context.SaveChangesAsync(cancellationToken);
    }
}
