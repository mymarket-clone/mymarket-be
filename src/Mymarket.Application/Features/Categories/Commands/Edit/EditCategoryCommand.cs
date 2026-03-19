using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Commands.Edit;

public record EditCategoryCommand(
    int Id,
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu,
    bool? BrandRequired,
    IFormFile? Logo
) : IRequest<Unit>;

public class EditCategoryCommandHandler(
    IApplicationDbContext context,
    IImageService imageService
) : IRequestHandler<EditCategoryCommand, Unit>
{
    public async Task<Unit> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        var category = await context.Categories
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException($"Category with id {request.Id} not found.");

        category.Name = request.Name;
        category.NameEn = request.NameEn;
        category.NameRu = request.NameRu;
        category.ParentId = request.ParentId;

        if (request.Logo is null || request.Logo.Length == 0)
        {
            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

        var oldLogo = category.Logo;
        var newImage = await imageService.UploadAsync(request.Logo, cancellationToken);

        try
        {
            await context.Images.AddAsync(newImage, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            category.LogoId = newImage.Id;
            category.Logo = newImage;
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            try
            {
                await imageService.DeleteAsync(newImage, cancellationToken);
            }
            catch {}

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