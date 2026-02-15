using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Edit;

public record EditCategoryAttributesCommand(
    int Id,
    int? AttributeId,
    bool? IsRequired,
    int? Order
) : IRequest<Unit>;

public class EditCategoryAttributesCommandHandler(
    IApplicationDbContext _context) : IRequestHandler<EditCategoryAttributesCommand, Unit>
{
    public async Task<Unit> Handle(EditCategoryAttributesCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.CategoryAttributes
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        category!.AttributeId = request.AttributeId ?? category.AttributeId;
        category!.IsRequired = request.IsRequired ?? category.IsRequired;
        category!.Order = request.Order ?? category.Order;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
