using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common;
using Mymarket.Application.features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.Get;

public record GetCategoriesQuery : IRequest<IEnumerable<CategoryTreeDto>>;

public class GetCategoriesQueryHandler(
    IApplicationDbContext _context,
    IMapper _mapper,
    ILanguageContext _languageContext) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryTreeDto>>
{
    public async Task<IEnumerable<CategoryTreeDto>> Handle(
        GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtoById = entities.ToDictionary(
            x => x.Id,
            x =>
            {
                var dto = _mapper.Map<CategoryTreeDto>(
                    x,
                    opt => opt.Items["lang"] = _languageContext.Language);

                dto.Children = null;
                return dto;
            });

        return TreeBuilder.Build(
            entities,
            dtoById,
            x => x.Id,
            x => x.ParentId
        );
    }
}