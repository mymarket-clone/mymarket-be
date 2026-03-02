using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Commands.Delete;

public record DeleteBrandCommand(
    int Id
) : IRequest<Unit>;

public class DeleteBrandCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteBrandCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
          .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        context.Brands.Remove(brand!);

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


