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
    ) : IRequest<CategoryBrandDto?>;

    public class GetCategoryBrandsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper) : IRequestHandler<GetCategoryBrandsQuery, CategoryBrandDto?>
    {
        public Task<CategoryBrandDto?> Handle(GetCategoryBrandsQuery request, CancellationToken cancellationToken)
        {
            var categoryBrand = context.CategoryBrands
                .AsNoTracking()
                .ProjectTo<CategoryBrandDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.CategoryId == request.Id, cancellationToken);

            return categoryBrand;
        }
    }
}