using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Categories.Queries.GetByIdWithChildren;

public record GetCategoriesByIdWithChildrenQuery(int Id) : IRequest<CategoryDto[]?>;

public class GetCategoriesByIdWithChildrenQueryHandler(
    IApplicationDbContext _context,
    IConfigurationProvider _mapper) : IRequestHandler<GetCategoriesByIdWithChildrenQuery, CategoryDto[]?>
{
    public async Task<CategoryDto[]?> Handle(GetCategoriesByIdWithChildrenQuery request, CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories
             .AsNoTracking()
             .AnyAsync(c => c.Id == request.Id, cancellationToken);

        IQueryable<CategoryEntity> query = categoryExists
            ? _context.Categories.Where(c => c.ParentId == request.Id)
            : _context.Categories.Where(c => c.ParentId == null);

        var result = await query
            .AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper)
            .ToArrayAsync(cancellationToken);

        if (result.Length == 0)
            return null;

        return result;
    }
}
