using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.GetAttributes
{
    public record GetAttributesQuery: IRequest<List<CategoryAttributeDto>?>;

    public class GetCategoryAttributeByIdQueryHandler(
        IApplicationDbContext _context,
        IConfigurationProvider _mapper) : IRequestHandler<GetAttributesQuery, List<CategoryAttributeDto>?>
    {
        public async Task<List<CategoryAttributeDto>?> Handle(
            GetAttributesQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.CategoryAttributes
                .AsNoTracking()
                .ProjectTo<CategoryAttributeDto>(_mapper)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}