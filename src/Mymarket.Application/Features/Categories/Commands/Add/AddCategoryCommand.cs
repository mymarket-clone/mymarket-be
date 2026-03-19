using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Mymarket.Application.Features.Categories.Commands.Add;

public record AddCategoryCommand(
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu,
    bool? BrandRequired,
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
        var uploadedImage = await imageService.UploadAsync(request.Logo, cancellationToken);

        try
        {
            if (request.ParentId.HasValue)
            {
                var parentExists = await context.Categories
                    .AnyAsync(c => c.Id == request.ParentId.Value, cancellationToken);
            }

            var category = new CategoryEntity
            {
                ParentId = request.ParentId,
                Name = request.Name,
                NameEn = request.NameEn,
                NameRu = request.NameRu,
                BrandRequired = request.BrandRequired,
                CategoryPostType = request.CategoryPostType,
                Logo = new ImageEntity 
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
                CategoryPostType = category.CategoryPostType,
                LogoUrl = context.Images
                    .Where(i => i.Id == uploadedImage.Id)
                    .Select(i => i.Url)
                    .FirstOrDefault() ?? string.Empty
            };

            return dto;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            await imageService.DeleteAsync(uploadedImage, cancellationToken);
            throw;
        }
    }
}
