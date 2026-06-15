using MediatR;
using Mymarket.Application.Features.Pricing.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Pricing.Queries.Calculate;

public record CalculateListingPriceQuery(
    PromoType? PromoType,
    int? PromoDays,
    bool IsColored,
    int? ColorDays,
    bool AutoRenewal,
    int? AutoRenewalOnceIn
) : IRequest<ListingPriceCalculationDto>;

public class CalculateListingPriceQueryHandler(
    IListingPricingService listingPricingService) : IRequestHandler<CalculateListingPriceQuery, ListingPriceCalculationDto>
{
    public Task<ListingPriceCalculationDto> Handle(
        CalculateListingPriceQuery request,
        CancellationToken cancellationToken)
    {
        return listingPricingService.CalculateAsync(
            new ListingPricingSelection(
                request.PromoType,
                request.PromoDays,
                request.IsColored,
                request.ColorDays,
                request.AutoRenewal,
                request.AutoRenewalOnceIn),
            cancellationToken);
    }
}
