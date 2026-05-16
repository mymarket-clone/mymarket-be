using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Roles.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Roles.Commands.Edit;

public record EditRoleCommand(
    int Id,
    string Name,
    List<int>? PermissionIds
) : IRequest<RoleDto>;

public class EditRoleCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(EditRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (role is null)
        {
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);
        }

        var permissionIds = request.PermissionIds?.Distinct().ToList() ?? [];
        var permissions = await context.Permissions
            .Where(x => permissionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        role.Name = request.Name;
        role.Permissions.Clear();

        foreach (var permission in permissions)
        {
            role.Permissions.Add(permission);
        }

        await context.SaveChangesAsync(cancellationToken);

        return role.Adapt<RoleDto>();
    }
}
