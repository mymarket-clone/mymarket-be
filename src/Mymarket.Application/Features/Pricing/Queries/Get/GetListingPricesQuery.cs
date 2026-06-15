using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Pricing.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Pricing.Queries.Get;

public record GetListingPricesQuery : IRequest<ListingPriceCatalogDto>;

public class GetListingPricesQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetListingPricesQuery, ListingPriceCatalogDto>
{
    public async Task<ListingPriceCatalogDto> Handle(GetListingPricesQuery request, CancellationToken cancellationToken)
    {
        var prices = await context.ListingServicePrices
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.ServiceType)
            .ThenBy(x => x.FromDay)
            .Select(x => new ListingServicePriceDto(
                x.Id,
                x.ServiceType,
                x.FromDay,
                x.ToDay,
                x.PricePerDay,
                x.OriginalPricePerDay,
                x.IsActive))
            .ToListAsync(cancellationToken);

        return new ListingPriceCatalogDto(prices);
    }
}
