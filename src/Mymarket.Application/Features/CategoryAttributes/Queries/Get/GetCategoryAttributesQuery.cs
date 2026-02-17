using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.Get;

public record GetCategoryAttributesQuery : IRequest<List<CategoryAttributeOptionsDto>>;

public class GetCategoryAttributesQueryHandler(
    IApplicationDbContext _context) : IRequestHandler<GetCategoryAttributesQuery, List<CategoryAttributeOptionsDto>>
{
    public async Task<List<CategoryAttributeOptionsDto>> Handle(GetCategoryAttributesQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CategoryAttributes
            .AsNoTracking()
            .GroupJoin(
                _context.AttributesOptions,
                category => category.AttributeId,
                option => option.AttributeId,
                (category, options) => new CategoryAttributeOptionsDto
                {
                    Id = category.Id,
                    CategoryId = category.CategoryId,
                    AttributeId = category.AttributeId,
                    IsRequired = category.IsRequired,
                    Order = category.Order,
                    Options = options
                        .Select(x => new AttributeOptionDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Order = x.Order
                        })
                        .OrderBy(x => x.Order)
                        .ToList()
                }).OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return result;
    }
}
