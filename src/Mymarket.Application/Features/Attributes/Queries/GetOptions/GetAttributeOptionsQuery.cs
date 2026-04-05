using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.AttributeOptions.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.GetOptions;

public record GetAttributeOptionsQuery(
    int Id
) : IRequest<List<AttributeOptionDto>>;

public class GetAttributeOptionsQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetAttributeOptionsQuery, List<AttributeOptionDto>>
{
    public async Task<List<AttributeOptionDto>> Handle(GetAttributeOptionsQuery request, CancellationToken cancellationToken)
    {
        var result = await context.AttributeOptions
            .Where(x => x.AttributeId == request.Id)
            .ProjectTo<AttributeOptionDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return result;
    }
}