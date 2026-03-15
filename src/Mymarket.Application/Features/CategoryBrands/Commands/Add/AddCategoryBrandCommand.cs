using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Add;

public record AddCategoryBrandCommand(
    int CategoryId,
    int BrandId
) : IRequest<Unit>;

public class AddCategoryBrandCommandHandler(
    IApplicationDbContext context) : IRequestHandler<AddCategoryBrandCommand, Unit>
{
    public async Task<Unit> Handle(AddCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = new CategoryBrandsEntity
        {
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
        };

        await context.CategoryBrands.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

