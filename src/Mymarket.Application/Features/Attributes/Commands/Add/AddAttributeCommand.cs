using AutoMapper;
using MediatR;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Features.Units.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Attributes.Commands.Add;

public record AddAttributeCommand(
    string Name,
    string NameEn,
    string NameRu,
    string Code,
    int? UnitId,
    AttributeType AttributeType
) : IRequest<AttributeDto>;

public class AddAttributeCommandHandler(
    IApplicationDbContext _context, IMapper _mapper) : IRequestHandler<AddAttributeCommand, AttributeDto>
{
    public async Task<AttributeDto> Handle(AddAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = new AttributeEntity
        {
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
            Code = request.Code,
            UnitId = request.UnitId,
            AttributeType = request.AttributeType
        };

        await _context.Attributes.AddAsync(attribute, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttributeDto>(attribute);
    }
}
