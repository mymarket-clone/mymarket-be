using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Mymarket.Application.Features.Brands.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Brands.Commands.Add;

public record AddBrandCommand(
    string Name,
    IFormFile Logo
) : IRequest<BrandDto>;

public class AddBrandCommandHandler(
    IApplicationDbContext context,
    IImageService imageService) : IRequestHandler<AddBrandCommand, BrandDto>
{
    public async Task<BrandDto> Handle(AddBrandCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);
        var uploadedImage = await imageService.UploadAsync(request.Logo, cancellationToken);

        try
        {
            await context.Images.AddAsync(uploadedImage, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var brand = new BrandEntity
            {
                Name = request.Name,
                LogoId = uploadedImage.Id,
            };

            await context.Brands.AddAsync(brand, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return new BrandDto
            {
                Id = brand.Id,
                Name = brand.Name,
                LogoId = uploadedImage.Id,
                LogoUrl = context.Images
                    .Where(i => i.Id == uploadedImage.Id)
                    .Select(i => i.Url)
                    .FirstOrDefault() ?? string.Empty
            };
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            await imageService.DeleteAsync(uploadedImage, cancellationToken);
            throw;
        }
    }
}
