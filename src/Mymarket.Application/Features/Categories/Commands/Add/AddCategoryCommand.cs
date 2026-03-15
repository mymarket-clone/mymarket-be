using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Enums;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Commands.Add;

public record AddCategoryCommand(
    int? ParentId,
    string Name,
    string? NameEn,
    string? NameRu,
    bool? BrandRequired,
    CategoryPostType CategoryPostType
) : IRequest<CategoryDto>;

public class AddCategoryCommandHandler(
    IApplicationDbContext context) : IRequestHandler<AddCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        AddCategoryCommand request, CancellationToken cancellationToken)
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
            CategoryPostType = request.CategoryPostType
        };

        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        var dto = new CategoryDto
        {
            Id = category.Id,
            ParentId = category.ParentId,
            Name = category.Name,
            NameEn = category.NameEn,
            NameRu = category.NameRu,
            BrandRequired = request.BrandRequired,
            CategoryPostType = category.CategoryPostType
        };

        return dto;
    }
}
