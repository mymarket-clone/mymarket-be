using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Brands.Commands.Delete;

public record DeleteBrandCommand(
    int Id
) : IRequest<Unit>;

public class DeleteBrandCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteBrandCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .FindAsync([request.Id], cancellationToken);

        if (brand is null)
        {
            throw new ApplicationException(SharedResources.RecordNotFound);
        }

        context.Brands.Remove(brand!);

        await context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}


