using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.HomeCategories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.HomeCategories.Commands.Edit;

public record EditHomeCategoryCommand(
    int Id,
    int CategoryId,
    int Order
) : IRequest<HomeCategoryDto>;

public class EditHomeCategoryCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditHomeCategoryCommand, HomeCategoryDto>
{
    public async Task<HomeCategoryDto> Handle(EditHomeCategoryCommand request, CancellationToken cancellationToken)
    {
        var homeCategory = await context.HomeCategories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (homeCategory is null)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        homeCategory.CategoryId = request.CategoryId;
        homeCategory.Order = request.Order;

        await context.SaveChangesAsync(cancellationToken);

        return new HomeCategoryDto
        {
            Id = homeCategory.Id,
            CategoryId = homeCategory.CategoryId,
            Order = homeCategory.Order
        };
    }
}