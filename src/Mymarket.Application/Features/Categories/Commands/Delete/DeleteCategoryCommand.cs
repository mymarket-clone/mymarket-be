using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Commands.Delete;

public record DeleteCategoryCommand(int Id) : IRequest<Unit>;

public class DeleteCategoryCommandHandler(IApplicationDbContext _context) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .Include(c => c.Children) 
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        _context.Categories.Remove(category!);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
