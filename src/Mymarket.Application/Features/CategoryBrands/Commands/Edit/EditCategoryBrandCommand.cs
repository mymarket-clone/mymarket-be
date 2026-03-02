using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Edit;

public record EditCategoryBrandCommand(
    int Id,
    int CategoryId,
    int BrandId
) : IRequest<Unit>;

public class EditCategoryBrandCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditCategoryBrandCommand, Unit>
{
    public async Task<Unit> Handle(EditCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var categoryBrand = await context.CategoryBrands
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        categoryBrand!.BrandId = request.BrandId;
        categoryBrand!.CategoryId = request.CategoryId;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
