using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.GetById;

public record GetCategoryAttributeByIdQuery(
    int Id
) : IRequest<List<CategoryAttributeDto>?>;

public class GetCategoryAttributeByIdQueryHandler(
    IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetCategoryAttributeByIdQuery, List<CategoryAttributeDto>?>
{
    public async Task<List<CategoryAttributeDto>?> Handle(GetCategoryAttributeByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CategoryAttributes
            .AsNoTracking()
            .Where(x => x.CategoryId == request.Id)
            .ProjectTo<CategoryAttributeDto>(_mapper)
            .ToListAsync(cancellationToken);

        return result;
    }
}