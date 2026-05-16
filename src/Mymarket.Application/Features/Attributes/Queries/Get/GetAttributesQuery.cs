using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.Get;

public record GetAttributesQuery: IRequest<List<AttributeDto>>;

public class GetAttributesQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetAttributesQuery, List<AttributeDto>>
{
    public async Task<List<AttributeDto>> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
    {
        var attributes = await context.Attributes
            .AsNoTracking()
            .ProjectToType<AttributeDto>()
            .ToListAsync(cancellationToken);

        return attributes;
    }
}
