using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.HomeCategories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.HomeCategories.Commands.Reorder;
public record ReorderHomeCategoriesCommand(
    List<HomeCategoryDto> Items
) : IRequest;

public class ReorderHomeCategoriesCommandHandler(
    IApplicationDbContext context
) : IRequestHandler<ReorderHomeCategoriesCommand>
{
    public async Task Handle(ReorderHomeCategoriesCommand request, CancellationToken cancellationToken)
    {
        if (request.Items is null || request.Items.Count == 0) return;

        var ids = request.Items.Select(x => x.Id).ToList();

        var homeCategories = await context.HomeCategories
            .Where(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (homeCategories.Count != request.Items.Count)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        var orderMap = request.Items.ToDictionary(x => x.Id, x => x.Order);

        foreach (var item in homeCategories)
        {
            item.Order = orderMap[item.Id];
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}