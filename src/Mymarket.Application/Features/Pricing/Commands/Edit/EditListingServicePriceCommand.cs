using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Pricing.Commands.Edit;

public record EditListingServicePriceCommand(
    int Id,
    int FromDay,
    int ToDay,
    decimal PricePerDay,
    bool IsActive
) : IRequest;

public class EditListingServicePriceCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditListingServicePriceCommand>
{
    public async Task Handle(EditListingServicePriceCommand request, CancellationToken cancellationToken)
    {
        var price = await context.ListingServicePrices
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        price.FromDay = request.FromDay;
        price.ToDay = request.ToDay;
        price.PricePerDay = request.PricePerDay;
        price.IsActive = request.IsActive;

        var prices = await context.ListingServicePrices
            .Where(x => x.ServiceType == price.ServiceType)
            .ToListAsync(cancellationToken);

        var originalPricePerDay = prices
            .Where(x => x.IsActive)
            .Select(x => (decimal?)x.PricePerDay)
            .Max()
            ?? prices.Select(x => (decimal?)x.PricePerDay).Max()
            ?? 0;

        foreach (var servicePrice in prices)
        {
            servicePrice.OriginalPricePerDay = originalPricePerDay;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
