using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Permissions.Queries.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Users.Queries.GetPermissions;

public record GetUserPermissionsQuery(int UserId) : IRequest<List<PermissionDto>?>;

public class GetUserPermissionsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetUserPermissionsQuery, List<PermissionDto>?>
{
    public async Task<List<PermissionDto>?> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .AsNoTracking()
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        return user?.Permissions
            .OrderBy(x => x.Id)
            .Adapt<List<PermissionDto>>();
    }
}
