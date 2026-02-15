using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCategoryCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .MustAsync(CategoryExists)
            .WithMessage(SharedResources.IdDoesnotExist);
    }

    private async Task<bool> CategoryExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id, cancellationToken);
    }
}