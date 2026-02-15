using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Delete;

public record DeleteCategoryAttributesCommand(
    int Id
): IRequest<Unit>;

public class DeleteCategoryAttributesCommandHandler(
    IApplicationDbContext _context) : IRequestHandler<DeleteCategoryAttributesCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryAttributesCommand request, CancellationToken cancellationToken)
    {
        var attribute = await _context.CategoryAttributes
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        _context.CategoryAttributes.Remove(attribute!);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
