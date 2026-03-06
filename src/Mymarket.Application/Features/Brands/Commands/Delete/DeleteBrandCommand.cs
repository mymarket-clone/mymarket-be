using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Commands.Delete;

public record DeleteBrandCommand(int Id) : IRequest<Unit>;

public class DeleteBrandCommandHandler(
    IApplicationDbContext context,
    IImageService imageService
) : IRequestHandler<DeleteBrandCommand, Unit>
{
    public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (brand is null)
            throw new KeyNotFoundException($"Brand with id {request.Id} not found.");

        var logo = brand.Logo;

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        try
        {
            context.Brands.Remove(brand);
            await context.SaveChangesAsync(cancellationToken);

            if (logo is not null)
            {
                context.Images.Remove(logo);
                await context.SaveChangesAsync(cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }

        if (logo is not null)
        {
            await imageService.DeleteAsync(logo, cancellationToken);
        }

        return Unit.Value;
    }
}