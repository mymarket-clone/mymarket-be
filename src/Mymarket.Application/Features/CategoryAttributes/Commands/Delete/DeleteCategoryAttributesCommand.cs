using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Delete;

public record DeleteCategoryAttributesCommand(int Id ): IRequest;

public class DeleteCategoryAttributesCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCategoryAttributesCommand>
{
    public async Task Handle(DeleteCategoryAttributesCommand request, CancellationToken cancellationToken)
    {
        var attribute = await context.CategoryAttributes
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        context.CategoryAttributes.Remove(attribute!);
        await context.SaveChangesAsync(cancellationToken);
    }
}
