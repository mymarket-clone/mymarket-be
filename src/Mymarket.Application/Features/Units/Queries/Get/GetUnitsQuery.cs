using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Queries.Get;

public record GetUnitsQuery: IRequest<List<UnitDto>>;

public class GetAllUnitQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetUnitsQuery, List<UnitDto>>
{
    public async Task<List<UnitDto>> Handle(GetUnitsQuery request, CancellationToken cancellationToken)
    {
        var result = await context.AttributeUnits
            .AsNoTracking()
            .ProjectTo<UnitDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return result;
    }
}
