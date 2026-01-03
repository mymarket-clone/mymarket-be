using FluentValidation;
using Mymarket.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Mymarket.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(CategoryExists)
            .WithMessage("Category with the specified Id does not exist.");

        RuleFor(x => x.Id)
            .MustAsync(CanDeleteCategory)
            .WithMessage("Cannot delete a category that has child categories.");
    }

    private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }

    private async Task<bool> CanDeleteCategory(int id, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(c => c.Children)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return category != null && (category.Children == null || !category.Children.Any());
    }
}