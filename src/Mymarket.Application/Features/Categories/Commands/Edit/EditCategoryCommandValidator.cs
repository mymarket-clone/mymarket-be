using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Categories.Commands.Edit;

public class AddCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category id is required")
            .MustAsync(CategoryExists).WithMessage("Category with this Id does not exist");

        RuleFor(x => x.ParentId)
            .MustAsync(ParentExists).WithMessage("Parent category does not exist")
            .MustAsync(NoCircularReference).WithMessage("Parent category cannot be a descendant of this category");

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);
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

    private async Task<bool> NoCircularReference(EditCategoryCommand command, int? parentId, CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return true;
        if (command.Id == parentId) return false;

        var descendants = await GetDescendants(command.Id, cancellationToken);

        return !descendants.Contains(parentId.Value);
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