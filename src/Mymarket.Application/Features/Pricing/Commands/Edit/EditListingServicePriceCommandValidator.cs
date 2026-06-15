using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Pricing.Commands.Edit;

public class EditListingServicePriceCommandValidator : AbstractValidator<EditListingServicePriceCommand>
{
    private readonly IApplicationDbContext _context;

    public EditListingServicePriceCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.FromDay)
            .GreaterThan(0).WithMessage("Min days must be greater than zero.");

        RuleFor(x => x.ToDay)
            .GreaterThan(0).WithMessage("Max days must be greater than zero.")
            .GreaterThanOrEqualTo(x => x.FromDay).WithMessage("Min days must be less than or equal to max days.");

        RuleFor(x => x.PricePerDay)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be zero or greater.");

        RuleFor(x => x)
            .MustAsync(DayRangeDoesNotOverlap)
            .WithMessage("Day range overlaps another tier for this service.");
    }

    private async Task<bool> DayRangeDoesNotOverlap(
        EditListingServicePriceCommand command,
        CancellationToken cancellationToken)
    {
        var current = await _context.ListingServicePrices
            .AsNoTracking()
            .Where(x => x.Id == command.Id)
            .Select(x => new { x.ServiceType })
            .FirstOrDefaultAsync(cancellationToken);

        if (current is null)
            return true;

        return !await _context.ListingServicePrices
            .AsNoTracking()
            .AnyAsync(x =>
                x.Id != command.Id &&
                x.ServiceType == current.ServiceType &&
                command.FromDay <= x.ToDay &&
                command.ToDay >= x.FromDay,
                cancellationToken);
    }
}
