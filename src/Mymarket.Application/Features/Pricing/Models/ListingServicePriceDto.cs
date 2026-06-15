using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Pricing.Models;

public record ListingServicePriceDto(
    int Id,
    ListingServiceType ServiceType,
    int FromDay,
    int ToDay,
    decimal PricePerDay,
    decimal? OriginalPricePerDay,
    bool IsActive
);
