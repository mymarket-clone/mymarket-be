using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Attributes.Commands.Delete;

public record DeleteAttributeCommand(
    int Id
) : IRequest<Unit>;

public class DeleteAttributeCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteAttributeCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.Attributes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attribute is null)
        {
            throw new ApplicationException(SharedResources.AttributeDoesnotExist);
        }

        _context.Attributes.Remove(attribute!);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
