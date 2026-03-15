using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Remove;

public record RemoveCategoryBrandCommand(
    int CategoryId,
    int BrandId
) : IRequest<Unit>;

public class RemoveCategoryBrandCommandHandler(
    IApplicationDbContext context) : IRequestHandler<RemoveCategoryBrandCommand, Unit>
{
    public async Task<Unit> Handle(RemoveCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var category = await context.CategoryBrands
                .FirstOrDefaultAsync(c => 
                    c.CategoryId == request.CategoryId &&
                    c.BrandId == request.BrandId, cancellationToken);

        var isUsedInPosts = await context.Posts
                .AnyAsync(
                    x => x.CategoryId == request.CategoryId &&
                         x.BrandId == request.BrandId, cancellationToken);

        if (isUsedInPosts)
        {
            throw new Exception("This brand cannot be removed from the category because it is used in existing posts.");
        }

        context.CategoryBrands.Remove(category!);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

