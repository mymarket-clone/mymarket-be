using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.HomeCategories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.HomeCategories.Queries.Get;

public record GetHomeCategoriesQuery : IRequest<List<HomeCategoryDto>>;

public class GetHomeCategoriesQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetHomeCategoriesQuery, List<HomeCategoryDto>>
{
    public async Task<List<HomeCategoryDto>> Handle(
        GetHomeCategoriesQuery request, CancellationToken cancellationToken)
    {
        var homeCategories = await context.HomeCategories
            .AsNoTracking()
            .ProjectTo<HomeCategoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return homeCategories;
    }
}
