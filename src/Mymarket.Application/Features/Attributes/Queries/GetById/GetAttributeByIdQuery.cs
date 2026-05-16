using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.GetById;

public record GetAttributeByIdQuery(int Id) : IRequest<AttributeDto?>;

public class GetAttributeByIdQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetAttributeByIdQuery, AttributeDto?>
{
    public async Task<AttributeDto?> Handle(GetAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        var attribute = await context.Attributes
            .AsNoTracking()
            .ProjectToType<AttributeDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return attribute is null ? null : attribute;
    }
}
