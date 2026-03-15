using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetBrands
{
    public record GetCategoryBrandsQuery(
        int Id
    ) : IRequest<List<CategoryBrandDto>?>;

    public class GetCategoryBrandsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetCategoryBrandsQuery, List<CategoryBrandDto>?>
    {
        public async Task<List<CategoryBrandDto>?> Handle(GetCategoryBrandsQuery request, CancellationToken cancellationToken)
        {
            var categoryBrand = await context.CategoryBrands
                .AsNoTracking()
                .ProjectTo<CategoryBrandDto>(mapper.ConfigurationProvider)
                .Where(x => x.CategoryId == request.Id)
                .ToListAsync(cancellationToken);

            return categoryBrand;
        }
    }
}