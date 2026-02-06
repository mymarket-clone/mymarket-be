using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Cities.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Cities.Queries;

public record GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>;

public class GetAllCitiesQueryHandler(IApplicationDbContext _context, IConfigurationProvider _mapper) 
    : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
{
    public async Task<IEnumerable<CityDto>> Handle( GetAllCitiesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Cities
            .AsNoTracking()
            .ProjectTo<CityDto>(_mapper)
            .ToListAsync(cancellationToken);
    }
}
