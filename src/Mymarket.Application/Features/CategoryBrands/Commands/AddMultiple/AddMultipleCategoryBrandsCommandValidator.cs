using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryBrands.Commands.AddMultiple;

public class AddMultipleCategoryBrandsCommandValidator : AbstractValidator<AddMultipleCategoryBrandsCommand>
{
    private readonly IApplicationDbContext _context;
    public AddMultipleCategoryBrandsCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.BrandIds)
            .MustAsync(AllBrandsExist).WithMessage(SharedResources.RecordNotFound)
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

    private async Task<bool> AllBrandsExist(List<int> brandIds, CancellationToken cancellationToken)
    {
        if (brandIds == null || brandIds.Count == 0)
            return false;

        var distinctIds = brandIds.Distinct().ToList();

        var existingCount = await _context.Brands
            .CountAsync(b => distinctIds.Contains(b.Id), cancellationToken);

        return existingCount == distinctIds.Count;
    }
}
