using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Permissions.Queries.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Permissions.Queries.Get;

public record GetPermissionsQuery : IRequest<List<PermissionDto>>;

public class GetPermissionsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetPermissionsQuery, List<PermissionDto>>
{
    public async Task<List<PermissionDto>> Handle(GetPermissionsQuery request, CancellationToken cancellationToken)
    {
        return await context.Permissions
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .ProjectToType<PermissionDto>()
            .ToListAsync(cancellationToken);
    }
}
