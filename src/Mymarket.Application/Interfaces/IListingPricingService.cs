using Mymarket.Application.Features.Pricing.Models;

namespace Mymarket.Application.Interfaces;

public interface IListingPricingService
{
    Task<ListingPriceCalculationDto> CalculateAsync(
        ListingPricingSelection selection,
        CancellationToken cancellationToken);
}
