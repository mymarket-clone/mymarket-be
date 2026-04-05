using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Delete;

public record DeleteAttributeOptionCommand(
    int Id
) : IRequest<Unit>;

public class DeleteAttributeOptionCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteAttributeOptionCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var attributeOption = await context.AttributeOptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);


        if (attributeOption is null)
        {
            throw new ApplicationException(SharedResources.AttributeOptionDoesnotExist);
        }

        context.AttributeOptions.Remove(attributeOption!);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}