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
            .Include(c => c.Logo)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        var logo = category.Logo;

        context.Categories.Remove(category!);
        await context.SaveChangesAsync(cancellationToken);

        if (logo is not null)
        {
            context.Images.Remove(logo);
            await context.SaveChangesAsync(cancellationToken);
            await imageService.DeleteAsync(logo, cancellationToken);
        }

        return Unit.Value;
    }
}
