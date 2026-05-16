using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Roles.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Roles.Commands.Add;

public record AddRoleCommand(
    string Name,
    List<int>? PermissionIds
) : IRequest<RoleDto>;

public class AddRoleCommnadHanlder(IApplicationDbContext context) : IRequestHandler<AddRoleCommand, RoleDto>
{
    public async Task<RoleDto> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        var permissionIds = request.PermissionIds?.Distinct().ToList() ?? [];
        var permissions = await context.Permissions
            .Where(x => permissionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var role = new RoleEntity
        {
            Name = request.Name,
            Permissions = permissions
        };

        await context.Roles.AddAsync(role, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return role.Adapt<RoleDto>();
    }
}
