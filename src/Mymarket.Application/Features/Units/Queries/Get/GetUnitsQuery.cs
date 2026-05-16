using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Queries.Get;

public record GetUnitsQuery: IRequest<List<UnitDto>>;

public class GetAllUnitQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetUnitsQuery, List<UnitDto>>
{
    public async Task<List<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var result = await context.AttributeUnits
            .AsNoTracking()
            .ProjectToType<UnitDto>()
            .ToListAsync(cancellationToken);

        return result;
    }
}
