using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.HomeCategories.Commands.Delete;

public record DeleteHomeCategoryCommand(
    int Id
) : IRequest<Unit>;

public class DeleteHomeCategoryCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteHomeCategoryCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteHomeCategoryCommand request, CancellationToken cancellationToken)
    {
        var homeCategory = await context.HomeCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (homeCategory is null)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        context.HomeCategories.Remove(homeCategory);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
