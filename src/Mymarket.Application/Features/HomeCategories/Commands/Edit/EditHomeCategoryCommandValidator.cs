using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.HomeCategories.Commands.Edit;

public class EditHomeCategoryCommandValidator : AbstractValidator<EditHomeCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public EditHomeCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(RecordExists).WithMessage(SharedResources.IdDoesnotExist);

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> RecordExists(int id, CancellationToken cancellationToken)
    {
        return await _context.HomeCategories.AnyAsync(x => x.Id == id, cancellationToken);
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
    }
}
