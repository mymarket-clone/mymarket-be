using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.HomeCategories.Commands.Add;

public record AddHomeCategoryCommand(
   int CategoryId,
   int Order
) : IRequest<HomeCategoriesEntity>;

public class AddHomeCategoryCommandHandler(
    IApplicationDbContext context) : IRequestHandler<AddHomeCategoryCommand, HomeCategoriesEntity>
{
    public async Task<HomeCategoriesEntity> Handle(AddHomeCategoryCommand request, CancellationToken cancellationToken)
    {
        var homeCategory = new HomeCategoriesEntity
        {
            CategoryId = request.CategoryId,
            Order = request.Order
        };
        context.HomeCategories.Add(homeCategory);
        await context.SaveChangesAsync(cancellationToken);

        return homeCategory;
    }
}
