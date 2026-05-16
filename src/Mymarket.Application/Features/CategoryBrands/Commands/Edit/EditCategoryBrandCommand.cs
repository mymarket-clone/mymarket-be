using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Edit;

public record EditCategoryBrandCommand(
    int Id,
    int CategoryId,
    int BrandId
) : IRequest;

public class EditCategoryBrandCommandHandler(IApplicationDbContext context) : IRequestHandler<EditCategoryBrandCommand>
{
    public async Task Handle(EditCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var categoryBrand = await context.CategoryBrands
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        categoryBrand!.BrandId = request.BrandId;
        categoryBrand!.CategoryId = request.CategoryId;

        await context.SaveChangesAsync(cancellationToken);
    }
}
