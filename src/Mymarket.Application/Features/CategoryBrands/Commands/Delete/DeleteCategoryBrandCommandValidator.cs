using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Commands.Delete;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Delete;

public class DeleteCategoryBrandCommandValidator : AbstractValidator<DeleteCategoryBrandCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryBrandCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(CategoryBrandsExists).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> CategoryBrandsExists(int id, CancellationToken cancellationToken)
    {
        return await _context.CategoryBrands.AnyAsync(c => c.Id == id, cancellationToken);
    }
}
