using AutoMapper;
using MediatR;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryBrands.Commands.AddMultiple;

public record AddMultipleCategoryBrandsCommand(
    int CategoryId,
    List<int> BrandIds
) : IRequest<List<CategoryBrandDto>>;

public class AddMultipleCategorBrandsCommandHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<AddMultipleCategoryBrandsCommand, List<CategoryBrandDto>>
{
    public async Task<List<CategoryBrandDto>> Handle(AddMultipleCategoryBrandsCommand request, CancellationToken cancellationToken)
    {
        var entities = new List<CategoryBrandsEntity>();

        foreach (var brandId in request.BrandIds)
        {
            var entity = new CategoryBrandsEntity
            {
                CategoryId = request.CategoryId,
                BrandId = brandId,
            };

            entities.Add(entity);
        }

        await context.CategoryBrands.AddRangeAsync(entities, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<List<CategoryBrandDto>>(entities);
    }
}
