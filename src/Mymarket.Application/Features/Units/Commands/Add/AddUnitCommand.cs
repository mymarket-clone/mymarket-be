using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Units.Commands.Add;

public record AddUnitCommand(
    string Name,
    string NameEn,
    string NameRu
): IRequest<Unit>;

public class AddUnitCommandhandler(IApplicationDbContext _context) : IRequestHandler<AddUnitCommand, Unit>
{
    public async Task<Unit> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        var AttributeUnit = new AttributeUnitEntity
        {
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
        };

        await _context.AttributeUnits.AddAsync(AttributeUnit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
