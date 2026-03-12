using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Commands.Delete;

public record DeleteCategoryCommand(
    int Id
) : IRequest<Unit>;

public class DeleteCategoryCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        context.Categories.Remove(category!);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
