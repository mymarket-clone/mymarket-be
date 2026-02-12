using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.AttributeOptions.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.AttributeOptions.Queries.GetAllById;

public record GetAllAttributeOptionsById(
    int Id
) : IRequest<List<AttributeOptionDto>>;

public class GetAllAttributeOptionsByIdHandler(
    IApplicationDbContext _context,
    IConfigurationProvider _mapper) : IRequestHandler<GetAllAttributeOptionsById, List<AttributeOptionDto>>
{
    public async Task<List<AttributeOptionDto>> Handle(GetAllAttributeOptionsById request, CancellationToken cancellationToken)
    {
        var result = await _context.AttributesOptions
            .Where(x => x.AttributeId == request.Id)
            .ProjectTo<AttributeOptionDto>(_mapper)
            .ToListAsync(cancellationToken);

        return result;
    }
}