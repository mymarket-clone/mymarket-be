using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetChildren;

public record GetCategoryChildren(int? Id) : IRequest<List<CategoryTreeAllDto>>;

public class GetCategoryChildrenHandler(
    IApplicationDbContext _context,
    IMapper _mapper) : IRequestHandler<GetCategoryChildren, List<CategoryTreeAllDto>>
{
    public async Task<List<CategoryTreeAllDto>> Handle(GetCategoryChildren request, CancellationToken cancellationToken)
    {
        var entities = await _context.Categories
            .Where(x => x.ParentId == request.Id)
            .Select(x => new CategoryTreeAllDto
            {
                Id = x.Id,
                Name = x.Name,
                NameEn = x.NameEn,
                NameRu = x.NameRu,
                HasChildren = _context.Categories.Any(c => c.ParentId == x.Id),
                CategoryPostType = x.CategoryPostType
            })
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CategoryTreeAllDto>>(entities);
    }
}