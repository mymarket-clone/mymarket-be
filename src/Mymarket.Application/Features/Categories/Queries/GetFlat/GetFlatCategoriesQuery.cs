using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetFlat;

public record GetFlatCategoriesQuery : IRequest<CategoryFlatDto[]>;

public class GetFlatCateogiresQueryHandler(
    IApplicationDbContext _context,
    ILanguageContext _languageContext,
    IMapper _mapper) : IRequestHandler<GetFlatCategoriesQuery, CategoryFlatDto[]>
{
    public async Task<CategoryFlatDto[]> Handle(
         GetFlatCategoriesQuery request,
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
            .ToArray();

        return categories;
    }
}
