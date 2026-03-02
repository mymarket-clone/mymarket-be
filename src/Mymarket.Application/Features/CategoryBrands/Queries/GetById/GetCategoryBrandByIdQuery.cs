using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Queries.GetById;

public record GetCategoryBrandByIdQuery(
    int Id
) : IRequest<CategoryBrandDto?>;

public class GetCategoryBrandByIdQueryHandler(
    IApplicationDbContext context, 
    IMapper mapper) : IRequestHandler<GetCategoryBrandByIdQuery, CategoryBrandDto?>
{
    public Task<CategoryBrandDto?> Handle(GetCategoryBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var categoryBrand = context.CategoryBrands
            .AsNoTracking()
            .ProjectTo<CategoryBrandDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return categoryBrand;
    }
}
