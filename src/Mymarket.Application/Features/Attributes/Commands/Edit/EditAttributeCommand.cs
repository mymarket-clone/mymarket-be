using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Attributes.Commands.Edit;

public record EditAttributeCommand(
    int Id,
    string Name,
    string NameEn,
    string NameRu,
    string Code,
    int? UnitId,
    AttributeType AttributeType
) : IRequest;

public class EditAttributeCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditAttributeCommand>
{
    public async Task Handle(EditAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await context.Attributes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attribute == null) {
            throw new KeyNotFoundException(SharedResources.RecordNotFound);
        }

        attribute!.Name = request.Name;
        attribute!.NameEn = request.NameEn;
        attribute!.NameRu = request.NameRu;
        attribute!.Code = request.Code;
        attribute!.UnitId = request.UnitId;
        attribute!.AttributeType = request.AttributeType;

        await context.SaveChangesAsync(cancellationToken);
    }
}
