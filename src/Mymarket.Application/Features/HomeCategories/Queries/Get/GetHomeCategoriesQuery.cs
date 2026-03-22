using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.HomeCategories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.HomeCategories.Queries.Get;

public record GetHomeCategoriesQuery : IRequest<List<HomeCategoryDto>>;

public class GetHomeCategoriesQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext) : IRequestHandler<GetHomeCategoriesQuery, List<HomeCategoryDto>>
{
    public async Task<List<HomeCategoryDto>> Handle(
        GetHomeCategoriesQuery request, CancellationToken cancellationToken)
    {
        var homeCategories = await context.HomeCategories
            .AsNoTracking()
            .OrderBy(x => x.Order)
            .Select(x => new HomeCategoryDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Order = x.Order,
                LogoUrl = x.Category != null && x.Category.Logo != null ? x.Category.Logo.Url : null,
                Name = languageContext.LocalizeProperty<CategoryEntity>("Name")(x.Category)
            })
            .ToListAsync(cancellationToken);

        return homeCategories;
    }
}
