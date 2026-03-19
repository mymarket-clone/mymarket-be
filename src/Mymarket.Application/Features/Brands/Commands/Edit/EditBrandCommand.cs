using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Brands.Commands.Edit;

public record EditBrandCommand(
    int Id,
    string Name,
    IFormFile? Logo
) : IRequest<BrandDto>;

public class EditBrandCommandHandler(
    IApplicationDbContext context,
    IImageService imageService
) : IRequestHandler<EditBrandCommand, BrandDto>
{
    public async Task<BrandDto> Handle(EditBrandCommand request, CancellationToken cancellationToken)
    {
        var brand = await context.Brands
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (brand is null)
            throw new KeyNotFoundException($"Brand with id {request.Id} not found.");

        brand.Name = request.Name;

        var oldLogo = brand.Logo;

        if (request.Logo is null || request.Logo.Length == 0)
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            var newImage = await imageService.UploadAsync(request.Logo, cancellationToken);
            await using var transaction = await context.BeginTransactionAsync(cancellationToken);

            try
            {
                brand.Logo = new ImageEntity {
                    UniqueId = newImage.UniqueId,
                    Url = newImage.Url 
                };

                context.Images.Remove(oldLogo);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                await imageService.DeleteAsync(oldLogo, cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                await imageService.DeleteAsync(newImage, cancellationToken);

                throw;
            }
        }

        return await context.Brands
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new BrandDto
            {
                Id = x.Id,
                Name = x.Name,
                LogoId = x.LogoId,
                LogoUrl = x.Logo != null ? x.Logo.Url ?? string.Empty : string.Empty
            })
            .FirstAsync(cancellationToken);
    }
}