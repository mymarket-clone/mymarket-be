using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Queries.Get;

public record GetCategoryBrandsQuery : IRequest<List<CategoryBrandDto>>;

public class GetCategoryBrandsQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetCategoryBrandsQuery, List<CategoryBrandDto>>
{
    public async Task<List<CategoryBrandDto>> Handle(GetCategoryBrandsQuery request, CancellationToken cancellationToken)
    {
        var categoryBrands = await context.CategoryBrands
            .AsNoTracking()
            .ProjectTo<CategoryBrandDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return categoryBrands;
    }
}
