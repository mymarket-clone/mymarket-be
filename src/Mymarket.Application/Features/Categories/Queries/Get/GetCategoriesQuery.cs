using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.Get;

public record GetCategoriesQuery : IRequest<List<CategoryFlatDto>>;

public class GetCategoriesQueryHandler(
    IApplicationDbContext context,
    ILanguageContext languageContext,
    IMapper mapper) : IRequestHandler<GetCategoriesQuery, List<CategoryFlatDto>>
{
    public async Task<List<CategoryFlatDto>> Handle(
         GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var entities = await context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var categories = entities
            .Select(c => mapper.Map<CategoryFlatDto>(
                c,
                opt => opt.Items["lang"] = languageContext.Language
            ))
            .ToList();

        return categories;
    }
}
