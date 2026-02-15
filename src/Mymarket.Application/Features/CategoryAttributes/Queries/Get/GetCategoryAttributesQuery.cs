using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.Get;

public record GetCategoryAttributesQuery : IRequest<List<CategoryAttributeDto>>;

public class GetCategoryAttributesQueryHandler(
    IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetCategoryAttributesQuery, List<CategoryAttributeDto>>
{
    public async Task<List<CategoryAttributeDto>> Handle(GetCategoryAttributesQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CategoryAttributes
            .AsNoTracking()
            .ProjectTo<CategoryAttributeDto>(_mapper)
            .ToListAsync(cancellationToken);

        return result;
    }
}
