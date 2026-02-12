using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Edit;

public record EditAttributeOptionCommand(
    int Id,
    int Order,
    string Name,
    string? NameEn,
    string? NameRu
) : IRequest<Unit>;

public class EditAttributeOptionCommandHandler(IApplicationDbContext _context) : IRequestHandler<EditAttributeOptionCommand, Unit>
{
    public async Task<Unit> Handle(EditAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var attributeOption = await _context.AttributesOptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        attributeOption!.Order = request.Order;
        attributeOption!.Name = request.Name;
        attributeOption!.NameEn = request.NameEn;
        attributeOption!.NameRu = request.NameRu;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}