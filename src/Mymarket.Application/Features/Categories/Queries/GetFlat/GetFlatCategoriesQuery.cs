using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetFlat;
public record GetFlatCategoriesQuery : IRequest<CategoryDto[]>;

public class GetFlatCateogiresQueryHandler(
    IApplicationDbContext _context,
    IConfigurationProvider _mapper) : IRequestHandler<GetFlatCategoriesQuery, CategoryDto[]>
{
    public async Task<CategoryDto[]> Handle(GetFlatCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper)
            .ToArrayAsync(cancellationToken);

        return categories;
    }
}
