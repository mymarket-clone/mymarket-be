using Mapster;
using MediatR;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Units.Commands.Add;

public record AddUnitCommand(
    string Name,
    string NameEn,
    string NameRu
): IRequest<UnitDto>;

public class AddUnitCommandhandler(IApplicationDbContext context) : IRequestHandler<AddUnitCommand, UnitDto>
{
    public async Task<UnitDto> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        var attributeUnit = new AttributeUnitEntity
        {
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
        };

        await context.AttributeUnits.AddAsync(attributeUnit, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return attributeUnit.Adapt<UnitDto>();
    }
}
