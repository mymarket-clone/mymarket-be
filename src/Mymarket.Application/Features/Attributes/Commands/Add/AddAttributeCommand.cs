using AutoMapper;
using MediatR;
using Mymarket.Application.Features.Attributes.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;
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
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<AddAttributeCommand, AttributeDto>
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

        await context.Attributes.AddAsync(attribute, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<AttributeDto>(attribute);
    }
}
