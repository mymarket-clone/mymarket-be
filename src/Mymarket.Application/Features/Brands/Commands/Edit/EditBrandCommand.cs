using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Brands.Commands.Edit;

public record EditBrandCommand(
    int Id,
    string Name,
    IFormFile Logo
) : IRequest<Unit>;

public class EditBrandCommandHandler(
    IApplicationDbContext context,
    IImageService imageService
) : IRequestHandler<EditBrandCommand, Unit>
{
    public async Task<Unit> Handle(EditBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        var oldLogo = brand?.Logo;

        var newImage = await imageService.UploadAsync(request.Logo, cancellationToken);

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        try
        {
            brand!.Name = request.Name;

            await context.Images.AddAsync(newImage, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            brand.LogoId = newImage.Id;
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            await imageService.DeleteAsync(newImage, cancellationToken);
            throw;
        }

        if (oldLogo is not null)
        {
            try
            {
                await imageService.DeleteAsync(oldLogo, cancellationToken);
                context.Images.Remove(oldLogo);
                await context.SaveChangesAsync(cancellationToken);
            }
            catch {}
        }

        return Unit.Value;
    }
}