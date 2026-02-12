using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Queries.GetById;

public record GetUnitById(
    int Id
) : IRequest<UnitDto?>;

public class GetUnitByIdHandler(IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetUnitById, UnitDto?>
{
    public async Task<UnitDto?> Handle(GetUnitById request, CancellationToken cancellationToken)
    {
        var attribute = await _context.AttributeUnits
            .AsNoTracking()
            .ProjectTo<UnitDto>(_mapper)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attribute is null) return null;

        return attribute;
    }
}
