using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Roles.Commands.SetRolePermissions;

public record SetRolePermissionsCommand(
    int RoleId,
    List<int> Permissions
) : IRequest;

public class SetRolePermissionsCommandHandler(IApplicationDbContext context) : IRequestHandler<SetRolePermissionsCommand>
{
    public async Task Handle(SetRolePermissionsCommand request, CancellationToken cancellationToken)
    {
        var role = context.Roles
            .Include(x => x.Permissions)
            .FirstOrDefault(r => r.Id == request.RoleId);

        if (role == null)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        role.Permissions = await context.Permissions
            .Where(x => request.Permissions.Contains(x.Id))
            .ToListAsync(cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}

