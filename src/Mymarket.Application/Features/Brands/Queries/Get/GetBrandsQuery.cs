using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Queries.Get;

public record GetBrandsQuery : IRequest<List<BrandDto>>;

public class GetBrandsQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetBrandsQuery, List<BrandDto>>
{
    public Task<List<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = context.Brands
            .AsNoTracking()
            .ProjectTo<BrandDto>(mapper.ConfigurationProvider)
            .ToList();

        return Task.FromResult(brands);
    }
}