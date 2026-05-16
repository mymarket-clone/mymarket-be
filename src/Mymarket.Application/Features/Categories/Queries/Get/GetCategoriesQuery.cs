using EFCoreSecondLevelCacheInterceptor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.Get;

public record GetCategoriesQuery : IRequest<List<CategoryFlatDto>>;

public class GetCategoriesQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext) : IRequestHandler<GetCategoriesQuery, List<CategoryFlatDto>>
{
    public async Task<List<CategoryFlatDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await context.Categories
            .AsNoTracking()
            .Include(x => x.Logo)
            .Select(x => new CategoryFlatDto
            {
                Id = x.Id,
                ParentId = x.ParentId,
                CategoryPostType = x.CategoryPostType,
                LogoUrl = x.Logo != null ? x.Logo.Url : null,
                BrandRequired = x.BrandRequired,
                Name = languageContext.Language == "en" ? (x.NameEn ?? x.Name) :
                       languageContext.Language == "ru" ? (x.NameRu ?? x.Name) :
                       x.Name
            })
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .ToListAsync(cancellationToken);
    }
}
