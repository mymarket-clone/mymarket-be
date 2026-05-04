using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Units.Commands.Edit;

public record EditUnitCommand(
    int Id,
    string Name,
    string NameEn,
    string NameRu
) : IRequest<Unit>;

public class EditUnitCommandHandler(IApplicationDbContext context) : IRequestHandler<EditUnitCommand, Unit>
{
    public async Task<Unit> Handle(EditUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await context.AttributeUnits
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit == null)
        {
            throw new KeyNotFoundException(SharedResources.RecordNotFound);
        }


        unit!.Name = request.Name;
        unit!.NameEn = request.NameEn;
        unit!.NameRu = request.NameRu;

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
