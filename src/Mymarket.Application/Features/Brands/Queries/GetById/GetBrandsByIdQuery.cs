using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Queries.GetById;

public record GetBrandsByIdQuery(int Id) : IRequest<BrandDto?>;

public class GetBrandsByIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetBrandsByIdQuery, BrandDto?>
{
    public async Task<BrandDto?> Handle(GetBrandsByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .AsNoTracking()
            .ProjectTo<BrandDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return brand;
    }
}