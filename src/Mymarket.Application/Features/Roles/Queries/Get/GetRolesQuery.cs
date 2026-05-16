using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Roles.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Common;

namespace Mymarket.Application.Features.Roles.Queries.Get;

public record GetRolesQuery(
    int PageNumber,
    int PageSize
) : IRequest<PaginatedResult<RoleDto>>;

public class GetRolesQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetRolesQuery, PaginatedResult<RoleDto>>
{
    public async Task<PaginatedResult<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var query = context.Roles
            .AsNoTracking()
            .Include(x => x.Permissions);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var dtos = items.Adapt<List<RoleDto>>();

        return new PaginatedResult<RoleDto>
        {
            Items = dtos,
            TotalCount = totalCount,
            Page = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}

