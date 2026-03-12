using AutoMapper;
using MediatR;
using Mymarket.Application.Features.AttributeOptions.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Add;

public record AddAttributeOptionCommand(
    int AttributeId,
    int Order,
    string Name,
    string? NameEn,
    string? NameRu
) : IRequest<AttributeOptionDto>;

public class AddAttributeOptionCommandHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<AddAttributeOptionCommand, AttributeOptionDto>
{
    public async Task<AttributeOptionDto> Handle(
        AddAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var AttributeOption = new AttributeOptionsEntity
        {
            AttributeId = request.AttributeId,
            Order = request.Order,
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
        };

        await context.AttributesOptions.AddAsync(AttributeOption, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<AttributeOptionDto>(AttributeOption);
    }
}
