using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Cities.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Cities.Queries;

public record GetCitiesQuery : IRequest<List<CityDto>>;

public class GetAllCitiesQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetCitiesQuery, List<CityDto>>
{
    public async Task<List<CityDto>> Handle( GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var result = await context.Cities
            .AsNoTracking()
            .ProjectTo<CityDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return result;
    }
}
