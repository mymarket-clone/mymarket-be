using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Constants;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Attributes.Commands.Add;

public record AddAttributeCommand(
    string Name,
    string NameEn,
    string NameRu,
    string Code,
    bool IsRequired,
    AttributeType AttributeType
) : IRequest<Unit>;

public class AddAttributeCommandHandler(
    IApplicationDbContext _context) : IRequestHandler<AddAttributeCommand, Unit>
{
    public async Task<Unit> Handle(AddAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = new AttributeEntity
        {
            Name = request.Name,
            NameEn = request.NameEn,
            NameRu = request.NameRu,
            Code = request.Code,
            IsRequired = request.IsRequired,
            AttributeType = request.AttributeType
        };

        await _context.Attributes.AddAsync(attribute, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
