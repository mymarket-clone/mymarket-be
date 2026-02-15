using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Delete;

public class DeleteCategoryAttributesCommandValidator : AbstractValidator<DeleteCategoryAttributesCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryAttributesCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(CategoryAttributesExists)
            .WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> CategoryAttributesExists(int id, CancellationToken cancellationToken)
    {
        return await _context.CategoryAttributes.AnyAsync(c => c.Id == id, cancellationToken);
    }
}
