using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Delete;

public record DeleteAttributeOptionCommand(
    int Id
) : IRequest<Unit>;

public class DeleteAttributeOptionCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteAttributeOptionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var attributeOption = await _context.AttributesOptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


        if (attributeOption is null)
        {
            throw new ApplicationException(SharedResources.AttributeOptionDoesnotExist);
        }

        _context.AttributesOptions.Remove(attributeOption!);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}