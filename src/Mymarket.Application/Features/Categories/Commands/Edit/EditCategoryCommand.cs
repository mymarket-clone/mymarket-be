using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Commands.Edit;

public record EditCategoryCommand(
    int Id,
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu
) : IRequest<Unit>;

public class EditCategoryCommandHandler(IApplicationDbContext _context) : IRequestHandler<EditCategoryCommand, Unit>
{
    public async Task<Unit> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        category!.Name = request.Name;
        category!.NameEn = request.NameEn;
        category!.NameRu = request.NameRu;
        category!.ParentId = request.ParentId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
