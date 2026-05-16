using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetLocalized;

public record GetCategoriesLocalizedQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesLocalizedHandler(
    IApplicationDbContext context) : IRequestHandler<GetCategoriesLocalizedQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesLocalizedQuery request, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                CategoryPostType = x.CategoryPostType,
                BrandRequired = x.BrandRequired,
                BrandVisible = x.BrandVisible,
                HasChildren = x.Children.Any(),
                LogoUrl = x.Logo != null ? x.Logo.Url : null,
                Name = x.Name,  
                NameEn = x.NameEn,
                NameRu = x.NameRu,
            })
            .ToListAsync(cancellationToken);
    }
}
