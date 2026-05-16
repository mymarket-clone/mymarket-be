using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.AttributeOptions.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.GetOptions;

public record GetAttributeOptionsQuery(int Id) : IRequest<List<AttributeOptionDto>>;

public class GetAttributeOptionsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetAttributeOptionsQuery, List<AttributeOptionDto>>
{
    public async Task<List<AttributeOptionDto>> Handle(GetAttributeOptionsQuery request, CancellationToken cancellationToken)
    {
        var result = await context.AttributeOptions
            .AsNoTracking()
            .Where(x => x.AttributeId == request.Id)
            .ProjectToType<AttributeOptionDto>()
            .ToListAsync(cancellationToken);

        return result;
    }
}