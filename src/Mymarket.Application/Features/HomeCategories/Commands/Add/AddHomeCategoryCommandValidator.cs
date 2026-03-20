using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.HomeCategories.Commands.Add;

public class AddHomeCategoryCommandValidator : AbstractValidator<AddHomeCategoryCommand>
{
    private readonly IApplicationDbContext _context;
    public AddHomeCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .MustAsync(CategoryExists).WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> CategoryExists(int categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(x => x.Id == categoryId, cancellationToken);
    }
}
