namespace Mymarket.Application.Features.Pricing.Models;

public record ListingPriceCatalogDto(
    IReadOnlyCollection<ListingServicePriceDto> Prices
);
