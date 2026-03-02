using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Categories.Commands.Add;

public class AddCategoryCommandValidator : AbstractValidator<AddCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public AddCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.ParentId)
            .MustAsync(ParentExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.CategoryPostType)
            .IsInEnum()
            .NotEmpty().WithMessage(SharedResources.CategoryPostTypeRequired)
            .MustAsync(HaveSamePostTypeAsParent).WithMessage(SharedResources.CategoryPostTypeMismatch);
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
