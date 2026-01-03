using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Categories.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Categories.Queries.GetById;

public record GetCategoryByIdQuery(int Id) : IRequest<CategoryDto?>;

public class GetCategoryByIdQueryHandler(
    IApplicationDbContext _context,
    IConfigurationProvider _mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .AsNoTracking()
            .ProjectTo<CategoryDto>(_mapper)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return category;
    }
}
