using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;

namespace Mymarket.Application.Features.Attributes.Commands.Edit;

public record EditAttributeCommand(
    int Id,
    string Name,
    string NameEn,
    string NameRu,
    string Code,
    int? UnitId,
    AttributeType AttributeType
) : IRequest<Unit>;

public class EditAttributeCommandHandler(IApplicationDbContext _context) : IRequestHandler<EditAttributeCommand, Unit>
{
    public async Task<Unit> Handle(EditAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        attribute!.Name = request.Name;
        attribute!.NameEn = request.NameEn;
        attribute!.NameRu = request.NameRu;
        attribute!.Code = request.Code;
        attribute!.UnitId = request.UnitId;
        attribute!.AttributeType = request.AttributeType;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
