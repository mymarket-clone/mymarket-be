using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Edit;

public class EditAttributeOptionCommandValidator : AbstractValidator<EditAttributeOptionCommand>
{
    private readonly IApplicationDbContext _context;

    public EditAttributeOptionCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(OptionExists).WithMessage(SharedResources.IdDoesnotExist);

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

    private async Task<bool> OptionExists(int id, CancellationToken cancellationToken)
    {
        return await _context.AttributesOptions.AnyAsync(o => o.Id == id, cancellationToken);
    }
}
