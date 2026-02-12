using AutoMapper;
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

public class AddUnitCommandhandler(IApplicationDbContext _context, IMapper _mapper) : IRequestHandler<AddUnitCommand, UnitDto>
{
    public async Task<UnitDto> Handle(AddUnitCommand request, CancellationToken cancellationToken)
    {
        var AttributeUnit = new AttributeUnitEntity
        {
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
        };

        await _context.AttributeUnits.AddAsync(AttributeUnit, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UnitDto>(AttributeUnit);
    }
}
