using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Queries.GetAll;

public record GetAllUnitQuery: IRequest<List<UnitDto>>;

public class GetAllUnitQueryHandler(IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetAllUnitQuery, List<UnitDto>>
{
    public async Task<List<UnitDto>> Handle(GetAllUnitQuery request, CancellationToken cancellationToken)
    {
        var attributes = await _context.AttributeUnits
            .AsNoTracking()
            .ProjectTo<UnitDto>(_mapper)
            .ToListAsync(cancellationToken);

        return attributes;
    }
}
