using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Add;

public class AddAttributeOptionCommandValidator : AbstractValidator<AddAttributeOptionCommand>
{
    private readonly IApplicationDbContext _context;

    public AddAttributeOptionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.AttributeId)
            .NotEmpty().WithMessage(SharedResources.AttributeIdRequired)
            .MustAsync(AttributeExists).WithMessage(SharedResources.AttributeDoesnotExist)
            .MustAsync(AttributeTypeIsValid).WithMessage(SharedResources.AttributeTypeInvalid);

        RuleFor(x => x.Order)
            .NotEmpty().WithMessage(SharedResources.OrderRequired);

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);
    }

    private async Task<bool> AttributeExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Attributes.AnyAsync(c => c.Id == id, cancellationToken);
    }

    private async Task<bool> AttributeTypeIsValid(int id, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        if (attribute == null) return true;

        return attribute.AttributeType == AttributeType.Select;
    }
}
