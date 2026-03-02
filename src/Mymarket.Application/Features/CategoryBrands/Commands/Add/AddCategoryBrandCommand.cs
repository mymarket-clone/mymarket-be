using AutoMapper;
using MediatR;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryBrands.Commands.Add;

public record AddCategoryBrandCommand(
    int CategoryId,
    int BrandId
) : IRequest<CategoryBrandDto>;

public class AddCategoryBrandCommandHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<AddCategoryBrandCommand, CategoryBrandDto>
{
    public async Task<CategoryBrandDto> Handle(AddCategoryBrandCommand request, CancellationToken cancellationToken)
    {
        var entity = new CategoryBrandsEntity
        {
            CategoryId = request.CategoryId,
            BrandId = request.BrandId,
        };

        await context.CategoryBrands.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<CategoryBrandDto>(entity);
    }
}

