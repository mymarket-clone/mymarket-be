using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Delete;

public record DeleteCategoryBrandCommand(
    int Id
) : IRequest<Unit>;

public class DeleteCategoryBrandCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteCategoryBrandCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var category = await context.CategoryBrands
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        context.CategoryBrands.Remove(category!);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
