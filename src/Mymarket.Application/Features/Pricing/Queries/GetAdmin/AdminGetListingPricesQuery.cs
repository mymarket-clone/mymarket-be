using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Pricing.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Pricing.Queries.GetAdmin;

public record AdminGetListingPricesQuery : IRequest<IReadOnlyCollection<ListingServicePriceDto>>;

public class AdminGetListingPricesQueryHandler(
    IApplicationDbContext context) : IRequestHandler<AdminGetListingPricesQuery, IReadOnlyCollection<ListingServicePriceDto>>
{
    public async Task<IReadOnlyCollection<ListingServicePriceDto>> Handle(
        AdminGetListingPricesQuery request,
        CancellationToken cancellationToken)
    {
        return await context.ListingServicePrices
            .AsNoTracking()
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
    }
}
