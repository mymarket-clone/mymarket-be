using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Units.Commands.Delete;

public record DeleteUnitCommand(
    int Id
) : IRequest<Unit>;

public class DeleteUnitCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteUnitCommand, Unit>
{
    public async Task<Unit> Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await _context.AttributeUnits
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit is null)
        {
            throw new ApplicationException(SharedResources.RecordNotFound);
        }

        _context.AttributeUnits.Remove(unit!);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
