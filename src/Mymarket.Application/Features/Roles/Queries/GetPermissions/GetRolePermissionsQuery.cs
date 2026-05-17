using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Permissions.Queries.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Roles.Queries.GetPermissions;

public record GetRolePermissionsQuery(int RoleId) : IRequest<List<PermissionDto>?>;

public class GetRolePermissionsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetRolePermissionsQuery, List<PermissionDto>?>
{
    public async Task<List<PermissionDto>?> Handle(GetRolePermissionsQuery request, CancellationToken cancellationToken)
    {
        var role = await context.Roles
            .AsNoTracking()
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == request.RoleId, cancellationToken);

        return role?.Permissions
            .OrderBy(x => x.Id)
            .Adapt<List<PermissionDto>>();
    }
}
