using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using Mymarket.Domain.Entities;
using Mymarket.Domain.Enums;

namespace Mymarket.Application.Features.Categories.Commands.Edit;

public record EditCategoryCommand(
    int Id,
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu,
    bool BrandRequired,
    bool BrandVisible,
    CategoryPostType CategoryPostType,
    IFormFile? Logo
) : IRequest<CategoryDto>;

public class EditCategoryCommandHandler(
    IApplicationDbContext context,
    IImageService imageService
) : IRequestHandler<EditCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        var category = await context.Categories
            .Include(x => x.Logo)
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new KeyNotFoundException(SharedResources.IdDoesnotExist);

        category.Name = request.Name;
        category.NameEn = request.NameEn;
        category.NameRu = request.NameRu;
        category.ParentId = request.ParentId;
        category.BrandRequired = request.BrandRequired;
        category.BrandVisible = request.BrandVisible;
        category.CategoryPostType = request.CategoryPostType;

        if (request.Logo is null || request.Logo.Length == 0)
        {
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CategoryDto
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                NameEn = category.NameEn,
                NameRu = category.NameRu,
                BrandRequired = category.BrandRequired,
                BrandVisible = category.BrandVisible,
                CategoryPostType = category.CategoryPostType,
                LogoUrl = category.Logo?.Url
            };
        }

        var oldLogo = category.Logo;
        var newImage = await imageService.UploadAsync(request.Logo, cancellationToken);

        try
        {
            category.Logo = new ImageEntity
            {
                UniqueId = newImage.UniqueId,
                Url = newImage.Url
            };

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
            context.Images.Remove(oldLogo);
            await context.SaveChangesAsync(cancellationToken);
            await imageService.DeleteAsync(oldLogo, cancellationToken);
        }

        return new CategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            NameEn = category.NameEn,
            NameRu = category.NameRu,
            BrandRequired = category.BrandRequired,
            BrandVisible = category.BrandVisible,
            CategoryPostType = category.CategoryPostType,
            LogoUrl = category.Logo?.Url
        };
    }
}