using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Pricing.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Pricing.Commands.EditService;

public record EditListingServicePriceTierCommand(
    int Id,
    int FromDay,
    int ToDay,
    decimal PricePerDay,
    bool IsActive
);

public record EditListingServicePricesCommand(
    ListingServiceType ServiceType,
    IReadOnlyCollection<EditListingServicePriceTierCommand> Tiers
) : IRequest<IReadOnlyCollection<ListingServicePriceDto>>;

public class EditListingServicePricesCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditListingServicePricesCommand, IReadOnlyCollection<ListingServicePriceDto>>
{
    public async Task<IReadOnlyCollection<ListingServicePriceDto>> Handle(
        EditListingServicePricesCommand request,
        CancellationToken cancellationToken)
    {
        var prices = await context.ListingServicePrices
            .Where(x => x.ServiceType == request.ServiceType)
            .ToListAsync(cancellationToken);

        var existingTierIds = request.Tiers
            .Where(x => x.Id > 0)
            .Select(x => x.Id)
            .ToHashSet();

        var removedPrices = prices
            .Where(x => !existingTierIds.Contains(x.Id))
            .ToList();

        context.ListingServicePrices.RemoveRange(removedPrices);
        await context.SaveChangesAsync(cancellationToken);

        prices = await context.ListingServicePrices
            .Where(x => x.ServiceType == request.ServiceType)
            .ToListAsync(cancellationToken);

        var pricesById = prices.ToDictionary(x => x.Id);
        var servicePrices = new List<ListingServicePriceEntity>();

        foreach (var tier in request.Tiers)
        {
            if (tier.Id > 0 && pricesById.TryGetValue(tier.Id, out var existingPrice))
            {
                existingPrice.FromDay = tier.FromDay;
                existingPrice.ToDay = tier.ToDay;
                existingPrice.PricePerDay = tier.PricePerDay;
                existingPrice.IsActive = tier.IsActive;
                servicePrices.Add(existingPrice);
            }
            else
            {
                var newPrice = new ListingServicePriceEntity
                {
                    ServiceType = request.ServiceType,
                    FromDay = tier.FromDay,
                    ToDay = tier.ToDay,
                    PricePerDay = tier.PricePerDay,
                    IsActive = tier.IsActive
                };

                await context.ListingServicePrices.AddAsync(newPrice, cancellationToken);
                servicePrices.Add(newPrice);
            }
        }

        var originalPricePerDay = servicePrices
            .Where(x => x.IsActive)
            .Select(x => (decimal?)x.PricePerDay)
            .Max()
            ?? servicePrices.Select(x => (decimal?)x.PricePerDay).Max()
            ?? 0;

        foreach (var price in servicePrices)
        {
            price.OriginalPricePerDay = originalPricePerDay;
        }

        await context.SaveChangesAsync(cancellationToken);

        return await context.ListingServicePrices
            .AsNoTracking()
            .Where(x => x.ServiceType == request.ServiceType)
            .OrderBy(x => x.FromDay)
            .ThenBy(x => x.ToDay)
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
