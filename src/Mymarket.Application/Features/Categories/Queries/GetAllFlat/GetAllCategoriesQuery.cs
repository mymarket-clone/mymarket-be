using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetAllFlat;

public record GetAllCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetAllCategoriesQueryHandler(
    IApplicationDbContext _context, IConfigurationProvider _mapper) : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .ProjectTo<CategoryDto>(_mapper)
            .ToListAsync(cancellationToken);

        return categories;
    }
}
