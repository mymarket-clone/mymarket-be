using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Attributes.Queries.GetById;

public record GetAttributeById(
    int Id
) : IRequest<AttributeDto?>;

public class GetAttributeByIdHandler(IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetAttributeById, AttributeDto?>
{
    public async Task<AttributeDto?> Handle(GetAttributeById request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .AsNoTracking()
            .ProjectTo<AttributeDto>(_mapper)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attribute is null) return null;

        return attribute;
    }
}
