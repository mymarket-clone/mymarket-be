using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Edit;

public class EditCategoryAttributesCommandValidator : AbstractValidator<EditCategoryAttributesCommand>
{
    private readonly IApplicationDbContext _context;

    public EditCategoryAttributesCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage(SharedResources.IdRequired)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.AttributeId)
            .MustAsync(AttributeExists)
            .When(x => x.AttributeId.HasValue)
            .WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.Order)
            .NotEmpty().WithMessage(SharedResources.OrderRequired);
    }

    private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
    {
        return await _context.CategoryAttributes
            .AnyAsync(c => c.Id == id, cancellationToken);
    }

    private async Task<bool> AttributeExists(int? attributeId, CancellationToken cancellationToken)
    {
        return await _context.Attributes
            .AnyAsync(a => a.Id == attributeId!.Value, cancellationToken);
    }
}
