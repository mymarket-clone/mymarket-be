using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Features.Categories.Commands.Add;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ParentId)
            .MustAsync(ParentExists).WithMessage("Parent category does not exist");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(72).WithMessage("Name cannot exceed 72 characters");

        RuleFor(x => x.NameEn)
            .MaximumLength(72).WithMessage("NameEn cannot exceed 72 characters");

        RuleFor(x => x.NameRu)
            .MaximumLength(72).WithMessage("NameRu cannot exceed 72 characters");

        RuleFor(x => x.CategoryPostType)
            .IsInEnum()
            .NotEmpty().WithMessage("Category post type is required")
            .MustAsync(HaveSamePostTypeAsParent).WithMessage("Category post type must match the parent category post type");
    }

    private async Task<bool> ParentExists(int? parentId, CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return true;
        return await _context.Categories.AnyAsync(c => c.Id == parentId.Value, cancellationToken);
    }

    private async Task<bool> HaveSamePostTypeAsParent(AddCategoryCommand command, CategoryPostType categoryPostType, CancellationToken cancellationToken)
    {
        if (!command.ParentId.HasValue) return true;

        var parent = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == command.ParentId.Value, cancellationToken);

        if (parent is null) return false;

        return parent.CategoryPostType == categoryPostType;
    }
}
