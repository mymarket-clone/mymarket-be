using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Queries.GetAttributes;

public record GetCategoryAttributesQuery(
    int Id
): IRequest<List<CategoryAttributeOptionsDto>>;

public class GetCategoryAttributesQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext) : IRequestHandler<GetCategoryAttributesQuery, List<CategoryAttributeOptionsDto>>
{
    public async Task<List<CategoryAttributeOptionsDto>> Handle(
        GetCategoryAttributesQuery request, CancellationToken cancellationToken)
    {
        var result = await context.CategoryAttributes
            .AsNoTracking()
            .Where(x => x.CategoryId == request.Id)
            .Join(
                context.Attributes,
                category => category.AttributeId,
                attribute => attribute.Id,
                (category, attribute) => new
                {
                    category.Id,
                    category.CategoryId,
                    category.AttributeId,
                    AttributeName = languageContext.LocalizeProperty<AttributeEntity>("Name")(attribute),
                    attribute.AttributeType,
                    category.IsRequired,
                    category.Order,
                    UnitName = languageContext.LocalizeProperty<AttributeUnitEntity>("Name")(attribute.Unit)
                }
            )
            .GroupJoin(
                context.AttributesOptions,
                combined => combined.AttributeId,
                option => option.AttributeId,
                (combined, options) => new CategoryAttributeOptionsDto
                {
                    Id = combined.Id,
                    CategoryId = combined.CategoryId,
                    AttributeId = combined.AttributeId,
                    AttributeName = combined.AttributeName,
                    AttributeType = combined.AttributeType,
                    UnitName = combined.UnitName,
                    IsRequired = combined.IsRequired,
                    Order = combined.Order,
                    Options = options
                        .OrderBy(x => x.Order)
                        .Select(x => new AttributeOptionDto
                        {
                            Id = x.Id,
                            Name = languageContext.LocalizeProperty<AttributeOptionsEntity>("Name")(x)!,
                            Order = x.Order
                        })
                        .ToList()
                })
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

        return result;
    }
}
