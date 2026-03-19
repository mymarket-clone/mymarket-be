using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Categories.Commands.Delete;

public record DeleteCategoryCommand(
    int Id
) : IRequest<Unit>;

public class DeleteCategoryCommandHandler(
    IApplicationDbContext context,
    IImageService imageService) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        context.Categories.Remove(category!);
        await context.SaveChangesAsync(cancellationToken);

        if (category?.Logo is not null)
        {
            context.Images.Remove(category!.Logo);
            await imageService.DeleteAsync(category!.Logo, cancellationToken);
        }

        return Unit.Value;
    }
}
