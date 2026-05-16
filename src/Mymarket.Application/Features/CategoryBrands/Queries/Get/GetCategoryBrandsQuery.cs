using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Queries.Get;

public record GetCategoryBrandsQuery(int? Id) : IRequest<List<CategoryBrandDto>>;

public class GetCategoryBrandsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetCategoryBrandsQuery, List<CategoryBrandDto>>
{
    public async Task<List<CategoryBrandDto>> Handle(GetCategoryBrandsQuery request, CancellationToken cancellationToken)
    {
        return await context.CategoryBrands
            .AsNoTracking()
            .Where(x => !request.Id.HasValue || x.Id == request.Id.Value)
            .ProjectToType<CategoryBrandDto>()
            .ToListAsync(cancellationToken);
    }
}
