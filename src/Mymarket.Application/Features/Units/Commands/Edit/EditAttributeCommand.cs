using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Units.Commands.Edit;

public record EditUnitCommand(
    int Id,
    string Name,
    string NameEn,
    string NameRu
) : IRequest<Unit>;

public class EditUnitCommandHandler(IApplicationDbContext _context) : IRequestHandler<EditUnitCommand, Unit>
{
    public async Task<Unit> Handle(EditUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        unit!.Name = request.Name;
        unit!.NameEn = request.NameEn;
        unit!.NameRu = request.NameRu;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
