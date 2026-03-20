using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Categories.Commands.Add;

public record AddCategoryCommand(
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu,
    bool BrandRequired,
    bool BrandVisible,
    IFormFile? Logo,
    CategoryPostType CategoryPostType
) : IRequest<CategoryDto>;

public class AddCategoryCommandHandler(
    IApplicationDbContext context,
    IImageService imageService) : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        AddCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        ImageEntity? uploadedImage = null;

        try
        {
            if (request.Logo is not null)
            {
                uploadedImage = await imageService.UploadAsync(request.Logo, cancellationToken);
            }

            var category = new CategoryEntity
            {
                ParentId = request.ParentId,
                Name = request.Name,
                NameEn = request.NameEn,
                NameRu = request.NameRu,
                BrandRequired = request.BrandRequired,
                BrandVisible = request.BrandVisible,
                CategoryPostType = request.CategoryPostType,
                Logo = uploadedImage is null 
                    ? null 
                    : new ImageEntity
                        {
                            UniqueId = uploadedImage.UniqueId,
                            Url = uploadedImage.Url,
                        }
            };

            await context.Categories.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            var dto = new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                NameEn = category.NameEn,
                NameRu = category.NameRu,
                BrandRequired = request.BrandRequired,
                BrandVisible = request.BrandVisible,
                CategoryPostType = category.CategoryPostType,
                LogoUrl = uploadedImage is null 
                    ? null
                    : context.Images
                        .Where(i => i.Id == category.LogoId)
                        .Select(i => i.Url)
                        .FirstOrDefault() ?? string.Empty
            };

            return dto;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);

            if (uploadedImage is not null)
            {
                await imageService.DeleteAsync(uploadedImage, cancellationToken);
            }

            throw;
        }
    }
}
