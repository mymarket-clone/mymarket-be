using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryBrands.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryBrands.Queries.GetById;

public record GetBrandsByCategoryIdQuery(
    int Id
) : IRequest<CategoryBrandDto?>;

public class GetBrandsByCategoryIdHandler(
    IApplicationDbContext context, 
    IMapper mapper) : IRequestHandler<GetBrandsByCategoryIdQuery, CategoryBrandDto?>
{
    public Task<CategoryBrandDto?> Handle(GetBrandsByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var categoryBrand = context.CategoryBrands
            .AsNoTracking()
            .ProjectTo<CategoryBrandDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.CategoryId == request.Id, cancellationToken);

        return categoryBrand;
    }
}
