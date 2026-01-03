using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Common;
using Mymarket.Application.features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetAll;

public record GetAllCategoriesQuery : IRequest<IReadOnlyList<CategoryTreeAllDto>>;

public class GetCategoriesAllQueryHandler(
    IApplicationDbContext _context,
    IMapper _mapper) : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryTreeAllDto>>
{
    public async Task<IReadOnlyList<CategoryTreeAllDto>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var entities = await _context.Categories
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtoById = entities.ToDictionary(
            x => x.Id,
            x =>
            {
                var dto = _mapper.Map<CategoryTreeAllDto>(x);
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
