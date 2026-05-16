using EFCoreSecondLevelCacheInterceptor;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Queries.Get;

public record GetBrandsQuery : IRequest<List<BrandDto>>;

public class GetBrandsQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetBrandsQuery, List<BrandDto>>
{
    public async Task<List<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await context.Brands
            .AsNoTracking()
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromMinutes(10))
            .ProjectToType<BrandDto>()
            .ToListAsync(cancellationToken);

        return brands;
    }
}