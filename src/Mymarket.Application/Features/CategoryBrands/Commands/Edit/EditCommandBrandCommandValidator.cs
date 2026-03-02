using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Edit;

public class EditCommandBrandCommandValidator : AbstractValidator<EditCategoryBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public EditCommandBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.BrandId)
            .MustAsync(BrandExists).WithMessage(SharedResources.RecordNotFound)
            .NotEmpty().WithMessage(SharedResources.IdRequired);

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists).WithMessage(SharedResources.RecordNotFound)
            .NotEmpty().WithMessage(SharedResources.IdRequired);
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == categoryId, cancellationToken);
    }

    private async Task<bool> BrandExists(int brandId, CancellationToken cancellationToken)
    {
        return await _context.Brands
            .AnyAsync(c => c.Id == brandId, cancellationToken);
    }
}
