namespace Mymarket.Application.Features.Pricing.Models;

public record ListingPriceCalculationDto(
    decimal TotalPrice,
    decimal TotalOriginalPrice
);
