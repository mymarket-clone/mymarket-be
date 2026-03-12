using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetLocalized;

public record GetCategoriesLocalizedQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesLocalizedHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetCategoriesLocalizedQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesLocalizedQuery request, CancellationToken cancellationToken)
    {
        var categories = await context.Categories
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return categories;
    }
}
