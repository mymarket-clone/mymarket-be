using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.Get;

public record GetCategoryAttributesQuery(
    int Id
): IRequest<List<CategoryAttributeOptionsDto>>;

public class GetCategoryAttributesQueryHandler(
    IApplicationDbContext _context,
    ILanguageContext _languageContext) : IRequestHandler<GetCategoryAttributesQuery, List<CategoryAttributeOptionsDto>>
{
    public async Task<List<CategoryAttributeOptionsDto>> Handle(GetCategoryAttributesQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.CategoryAttributes
            .AsNoTracking()
            .Where(x => x.CategoryId == request.Id)
            .Join(
                _context.Attributes,
                category => category.AttributeId,
                attribute => attribute.Id,
                (category, attribute) => new
                {
                    category.Id,
                    category.CategoryId,
                    category.AttributeId,
                    AttributeName = _languageContext.LocalizeProperty<AttributeEntity>("Name")(attribute),
                    attribute.AttributeType,
                    category.IsRequired,
                    category.Order
                }
            )
            .GroupJoin(
                _context.AttributesOptions,
                combined => combined.AttributeId,
                option => option.AttributeId,
                (combined, options) => new CategoryAttributeOptionsDto
                {
                    Id = combined.Id,
                    CategoryId = combined.CategoryId,
                    AttributeId = combined.AttributeId,
                    AttributeName = combined.AttributeName,
                    AttributeType = combined.AttributeType,
                    IsRequired = combined.IsRequired,
                    Order = combined.Order,
                    Options = options
                        .OrderBy(x => x.Order)
                        .Select(x => new AttributeOptionDto
                        {
                            Id = x.Id,
                            Name = _languageContext.LocalizeProperty<AttributesOptionsEntity>("Name")(x),
                            Order = x.Order
                        })
                        .ToList()
                })
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return result;
    }
}
