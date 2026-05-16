using EFCoreSecondLevelCacheInterceptor;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Cities.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Cities.Queries;

public record GetCitiesQuery : IRequest<List<CityDto>>;

public class GetAllCitiesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetCitiesQuery, List<CityDto>>
{
    public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        return await context.Cities
            .AsNoTracking()
            .Cacheable(CacheExpirationMode.Absolute, TimeSpan.FromHours(1))
            .ProjectToType<CityDto>()
            .ToListAsync(cancellationToken);
    }
}
