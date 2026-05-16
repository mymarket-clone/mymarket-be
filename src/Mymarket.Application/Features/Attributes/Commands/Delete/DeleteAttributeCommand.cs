using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Attributes.Commands.Delete;

public record DeleteAttributeCommand(int Id) : IRequest;

public class DeleteAttributeCommandHandler(
    IApplicationDbContext context) : IRequestHandler<DeleteAttributeCommand>
{
    public async Task Handle(DeleteAttributeCommand request, CancellationToken cancellationToken)
    {
        var attribute = await context.Attributes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attribute is null)
        {
            throw new ApplicationException(SharedResources.AttributeDoesnotExist);
        }

        context.Attributes.Remove(attribute!);
        await context.SaveChangesAsync(cancellationToken);
    }
}
