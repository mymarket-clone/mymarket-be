using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.CategoryAttributes.Queries.Get
{
    public record GetAttributesQuery: IRequest<List<CategoryAttributeDto>?>;

    public class GetCategoryAttributeByIdQueryHandler(IApplicationDbContext context) : IRequestHandler<GetAttributesQuery, List<CategoryAttributeDto>?>
    {
        public async Task<List<CategoryAttributeDto>?> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            return await context.CategoryAttributes
                .AsNoTracking()
                .ProjectToType<CategoryAttributeDto>()
                .ToListAsync(cancellationToken);
        }
    }
}