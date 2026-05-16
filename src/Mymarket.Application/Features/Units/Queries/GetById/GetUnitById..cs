using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Queries.GetById;

public record GetUnitByIdQuery(int Id) : IRequest<UnitDto?>;

public class GetUnitByIdQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetUnitByIdQuery, UnitDto?>
{
    public async Task<UnitDto?> Handle(GetUnitByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await context.AttributeUnits
            .AsNoTracking()
            .ProjectToType<UnitDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return result is null ? null : result;
    }
}
