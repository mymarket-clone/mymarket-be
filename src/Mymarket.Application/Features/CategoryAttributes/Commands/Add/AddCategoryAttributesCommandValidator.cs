using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Add;

public class AddCategoryAttributesCommandValidator : AbstractValidator<AddCategoryAttributesCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCategoryAttributesCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.AttributeId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(AttributeExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.Order)
            .NotEmpty().WithMessage(SharedResources.OrderRequired);
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == categoryId, cancellationToken);
    }

    private async Task<bool> AttributeExists(int attributeId, CancellationToken cancellationToken)
    {
        return await _context.Attributes.AnyAsync(a => a.Id == attributeId, cancellationToken);
    }
}