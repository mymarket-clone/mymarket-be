using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Pricing.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Services;

public class ListingPricingService(IApplicationDbContext context) : IListingPricingService
{
    public async Task<ListingPriceCalculationDto> CalculateAsync(
        ListingPricingSelection selection,
        CancellationToken cancellationToken)
    {
        var prices = await context.ListingServicePrices
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var total = 0m;
        var originalTotal = 0m;

        if (selection.PromoType.HasValue && selection.PromoDays.HasValue)
        {
            var serviceType = selection.PromoType.Value switch
            {
                PromoType.VIP => ListingServiceType.Vip,
                PromoType.VIP_PLUS => ListingServiceType.VipPlus,
                PromoType.SUPER_VIP => ListingServiceType.SuperVip,
                _ => throw Validation(nameof(selection.PromoType), "Invalid promo type.")
            };

            AddCharge(prices, serviceType, selection.PromoDays.Value, ref total, ref originalTotal);
        }

        if (selection.IsColored && selection.ColorDays.HasValue)
        {
            AddCharge(prices, ListingServiceType.Color, selection.ColorDays.Value, ref total, ref originalTotal);
        }

        if (selection.AutoRenewal && selection.AutoRenewalOnceIn.HasValue)
        {
            AddCharge(prices, ListingServiceType.AutoRenewal, selection.AutoRenewalOnceIn.Value, ref total, ref originalTotal);
        }

        return new ListingPriceCalculationDto(total, originalTotal);
    }

    private static void AddCharge(
        IReadOnlyCollection<ListingServicePriceEntity> prices,
        ListingServiceType serviceType,
        int days,
        ref decimal total,
        ref decimal originalTotal)
    {
        var servicePrices = prices
            .Where(x => x.ServiceType == serviceType)
            .ToList();

        var price = servicePrices
            .OrderBy(x => x.FromDay)
            .FirstOrDefault(x => days >= x.FromDay && days <= x.ToDay);

        if (price is null)
        {
            throw Validation(serviceType.ToString(), "Selected number of days is not available for this service.");
        }

        total += price.PricePerDay * days;
        originalTotal += servicePrices.Max(x => x.PricePerDay) * days;
    }

    private static ValidationException Validation(string propertyName, string message)
    {
        return new ValidationException([new ValidationFailure(propertyName, message)]);
    }
}
