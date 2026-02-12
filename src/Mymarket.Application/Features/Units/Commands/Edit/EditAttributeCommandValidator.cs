using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Units.Commands.Edit;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Attributes.Commands.Edit;

public class EditUnitCommandValidator : AbstractValidator<EditUnitCommand>
{
    private readonly IApplicationDbContext _context;

    public EditUnitCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.AttributeIdRequired)
            .MustAsync(UnitExists).WithMessage(SharedResources.AttributeDoesnotExist);

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);
    }

    private async Task<bool> UnitExists(int id, CancellationToken cancellationToken)
    {
        return await _context.AttributeUnits.AnyAsync(c => c.Id == id, cancellationToken);
    }
}


