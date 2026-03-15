using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Remove;
public class RemoveCategoryBrandCommandValidator : AbstractValidator<RemoveCategoryBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public RemoveCategoryBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.BrandId)
            .MustAsync(BrandExists).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }

    private async Task<bool> BrandExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Brands.AnyAsync(c => c.Id == id, cancellationToken);
    }
}
