using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Queries.GetById;

public record GetBrandsByIdQuery(int Id) : IRequest<BrandDto?>;

public class GetBrandsByIdQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetBrandsByIdQuery, BrandDto?>
{
    public async Task<BrandDto?> Handle(GetBrandsByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .AsNoTracking()
            .ProjectToType<BrandDto>()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return brand;
    }
}