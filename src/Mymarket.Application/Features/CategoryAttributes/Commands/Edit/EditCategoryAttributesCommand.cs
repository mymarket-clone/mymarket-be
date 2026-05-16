using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Edit;

public record EditCategoryAttributesCommand(
    int Id,
    int? AttributeId,
    bool? IsRequired,
    int? Order
) : IRequest;

public class EditCategoryAttributesCommandHandler(IApplicationDbContext context) : IRequestHandler<EditCategoryAttributesCommand>
{
    public async Task Handle(EditCategoryAttributesCommand request, CancellationToken cancellationToken)
    {
        var category = await context.CategoryAttributes
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        category!.AttributeId = request.AttributeId ?? category.AttributeId;
        category!.IsRequired = request.IsRequired ?? category.IsRequired;
        category!.Order = request.Order ?? category.Order;

        await context.SaveChangesAsync(cancellationToken);
    }
}
