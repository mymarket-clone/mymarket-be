using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

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
    }

    private async Task<bool> ParentExists(int? parentId, CancellationToken cancellationToken)
    {
        if (!parentId.HasValue) return true;
        return await _context.Categories.AnyAsync(c => c.Id == parentId.Value, cancellationToken);
    }
}
