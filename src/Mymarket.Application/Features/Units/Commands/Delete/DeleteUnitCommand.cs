using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Units.Commands.Delete;

public record DeleteUnitCommand(
    int Id
) : IRequest<Unit>;

public class DeleteUnitCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteUnitCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await context.AttributeUnits
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit is null)
        {
            throw new ApplicationException(SharedResources.RecordNotFound);
        }

        context.AttributeUnits.Remove(unit!);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
