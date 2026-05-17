using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetBrands;

public record GetCategoryBrandsQuery(int Id) : IRequest<List<CategoryBrandDto>?>;

public class GetCategoryBrandsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCategoryBrandsQuery, List<CategoryBrandDto>?>
{
    public async Task<List<CategoryBrandDto>?> Handle(GetCategoryBrandsQuery request, CancellationToken cancellationToken)
    {
        var categoryBrand = await context.CategoryBrands
            .AsNoTracking()
            .Where(x => x.CategoryId == request.Id)
            .Select(x => new CategoryBrandDto
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                BrandId = x.BrandId,
                Name = x.Brand.Name
            })
            .ToListAsync(cancellationToken);

        return categoryBrand;
    }
}