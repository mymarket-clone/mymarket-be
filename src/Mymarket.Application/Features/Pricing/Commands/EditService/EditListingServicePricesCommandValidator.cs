using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Pricing.Commands.EditService;

public class EditListingServicePricesCommandValidator : AbstractValidator<EditListingServicePricesCommand>
{
    private readonly IApplicationDbContext _context;

    public EditListingServicePricesCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Tiers)
            .NotEmpty().WithMessage("At least one tier is required.");

        RuleForEach(x => x.Tiers).ChildRules(tier =>
        {
            tier.RuleFor(x => x.FromDay)
                .GreaterThan(0).WithMessage("Min days must be greater than zero.");

            tier.RuleFor(x => x.ToDay)
                .GreaterThan(0).WithMessage("Max days must be greater than zero.")
                .GreaterThanOrEqualTo(x => x.FromDay).WithMessage("Min days must be less than or equal to max days.");

            tier.RuleFor(x => x.PricePerDay)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");
        });

        RuleFor(x => x)
            .Must(NoOverlappingRanges)
            .WithMessage("Day ranges should not overlap inside the same service.");

        RuleFor(x => x)
            .MustAsync(TiersBelongToService)
            .WithMessage("One or more tiers do not belong to this service.");
    }

    private static bool NoOverlappingRanges(EditListingServicePricesCommand command)
    {
        var ranges = command.Tiers
            .OrderBy(x => x.FromDay)
            .ThenBy(x => x.ToDay)
            .ToList();

        for (var i = 1; i < ranges.Count; i++)
        {
            if (ranges[i].FromDay <= ranges[i - 1].ToDay)
            {
                return false;
            }
        }

        return true;
    }

    private async Task<bool> TiersBelongToService(
        EditListingServicePricesCommand command,
        CancellationToken cancellationToken)
    {
        var ids = command.Tiers
            .Where(x => x.Id > 0)
            .Select(x => x.Id)
            .ToList();

        if (ids.Count == 0)
        {
            return true;
        }

        var count = await _context.ListingServicePrices
            .AsNoTracking()
            .CountAsync(x => ids.Contains(x.Id) && x.ServiceType == command.ServiceType, cancellationToken);

        return count == ids.Count;
    }
}
