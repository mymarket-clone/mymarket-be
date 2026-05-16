using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetAttributes;

public record GetCategoryAttributesQuery(int Id): IRequest<List<CategoryAttributeOptionsDto>>;

public class GetCategoryAttributesQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext) : IRequestHandler<GetCategoryAttributesQuery, List<CategoryAttributeOptionsDto>>
{
    public async Task<List<CategoryAttributeOptionsDto>> Handle(
        GetCategoryAttributesQuery request, CancellationToken cancellationToken)
    {
        var lang = languageContext.Language;

        return await context.CategoryAttributes
            .AsNoTracking()
            .Where(x => x.CategoryId == request.Id)
            .Join(context.Attributes,
                category => category.AttributeId,
                attribute => attribute.Id,
                (category, attribute) => new
                {
                    category.Id,
                    category.CategoryId,
                    category.AttributeId,
                    AttributeName = languageContext.Get(attribute.NameEn, attribute.NameRu, attribute.Name),
                    attribute.AttributeType,
                    category.IsRequired,
                    category.Order,
                    UnitName = languageContext.Get(attribute.Unit!.NameEn, attribute.Unit!.NameRu, attribute.Unit.Name)
                })
            .GroupJoin(
                context.AttributeOptions,
                x => x.AttributeId,
                option => option.AttributeId,
                (x, options) => new CategoryAttributeOptionsDto
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    AttributeId = x.AttributeId,
                    AttributeName = x.AttributeName,
                    AttributeType = x.AttributeType,
                    UnitName = x.UnitName,
                    IsRequired = x.IsRequired,
                    Order = x.Order,

                    Options = options
                        .OrderBy(o => o.Order)
                        .Select(o => new AttributeOptionDto
                        {
                            Id = o.Id,
                            Name =
                                lang == "en" ? (o.NameEn ?? o.Name) :
                                lang == "ru" ? (o.NameRu ?? o.Name) :
                                o.Name,
                            Order = o.Order
                        })
                        .ToList()
                })
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);
    }
}
