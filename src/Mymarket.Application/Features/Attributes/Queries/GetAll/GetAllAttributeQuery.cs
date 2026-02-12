using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.GetAll;

public record GetAllAttributeQuery: IRequest<List<AttributeDto>>;

public class GetAllAttributeQueryHandler(IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetAllAttributeQuery, List<AttributeDto>>
{
    public async Task<List<AttributeDto>> Handle(GetAllAttributeQuery request, CancellationToken cancellationToken)
    {
        var attributes = await _context.Attributes
            .AsNoTracking()
            .ProjectTo<AttributeDto>(_mapper)
            .ToListAsync(cancellationToken);

        return attributes;
    }
}
