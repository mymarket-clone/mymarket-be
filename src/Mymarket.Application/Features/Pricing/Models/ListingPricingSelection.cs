using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Pricing.Models;

public record ListingPricingSelection(
    PromoType? PromoType,
    int? PromoDays,
    bool IsColored,
    int? ColorDays,
    bool AutoRenewal,
    int? AutoRenewalOnceIn
);
