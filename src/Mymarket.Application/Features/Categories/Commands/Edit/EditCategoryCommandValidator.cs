using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Categories.Commands.Edit;

public class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public EditCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.ParentId)
            .MustAsync(ParentExists).WithMessage(SharedResources.IdRequired)
            .MustAsync(NoCircularReference).WithMessage("Parent category cannot be a descendant of this category");

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.BrandRequired)
            .MustAsync(HaveValidBrandRequired)
            .WithMessage("BrandRequired must be null for non-leaf categories and required for leaf categories.");
    }

    private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }

    private async Task<bool> ParentExists(int? parentId, CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return true;

        return await _context.Categories.AnyAsync(c => c.Id == parentId.Value, cancellationToken);
    }

    private async Task<bool> NoCircularReference(
        EditCategoryCommand command,
        int? parentId,
        CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return true;
        if (command.Id == parentId) return false;

        var descendants = await GetDescendants(command.Id, cancellationToken);

        return !descendants.Contains(parentId.Value);
    }

    private async Task<bool> HaveValidBrandRequired(
        EditCategoryCommand command,
        bool? brandRequired,
        CancellationToken cancellationToken)
    {
        var hasChildren = await _context.Categories
            .AnyAsync(c => c.ParentId == command.Id, cancellationToken);

        if (hasChildren)
            return brandRequired is null;

        return brandRequired is not null;
    }

    private async Task<List<int>> GetDescendants(int categoryId, CancellationToken cancellationToken)
    {
        var result = new List<int>();

        var children = await _context.Categories
            .Where(c => c.ParentId == categoryId)
            .Select(c => c.Id)
            .ToListAsync(cancellationToken);

        foreach (var childId in children)
        {
            result.Add(childId);
            var childDescendants = await GetDescendants(childId, cancellationToken);
            result.AddRange(childDescendants);
        }

        return result;
    }
}