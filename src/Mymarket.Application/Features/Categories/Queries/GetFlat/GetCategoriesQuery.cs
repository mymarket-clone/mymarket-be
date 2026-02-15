using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetFlat;

public record GetCategoriesQuery : IRequest<List<CategoryFlatDto>>;

public class GetCategoriesQueryHandler(
    IApplicationDbContext _context,
    ILanguageContext _languageContext,
    IMapper _mapper) : IRequestHandler<GetCategoriesQuery, List<CategoryFlatDto>>
{
    public async Task<List<CategoryFlatDto>> Handle(
         GetCategoriesQuery request,
         CancellationToken cancellationToken)
    {
        var entities = await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var categories = entities
            .Select(c => _mapper.Map<CategoryFlatDto>(
                c,
                opt => opt.Items["lang"] = _languageContext.Language
            ))
            .ToList();

        return categories;
    }
}
