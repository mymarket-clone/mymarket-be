using MediatR;
using Mymarket.Application.Features.CategoryAttributes.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.CategoryAttributes.Commands.Add;

public record AddCategoryAttributesCommand(
    int CategoryId,
    int AttributeId,
    bool IsRequired,
    int Order
) : IRequest<CategoryAttributeDto>;

public class AddCategoryAttributesCommandHandler(IApplicationDbContext _context) : IRequestHandler<AddCategoryAttributesCommand, CategoryAttributeDto>
{
    public async Task<CategoryAttributeDto> Handle(AddCategoryAttributesCommand request, CancellationToken cancellationToken)
    {
        var entity = new CategoryAttributesEntity
        {
            CategoryId = request.CategoryId,
            AttributeId = request.AttributeId,
            IsRequired = request.IsRequired,
            Order = request.Order
        };

        await _context.CategoryAttributes.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = new CategoryAttributeDto
        {
            Id = entity.Id,
            CategoryId = entity.CategoryId,
            AttributeId = entity.AttributeId,
            IsRequired = entity.IsRequired,
            Order = entity.Order
        };

        return dto;
    }
}
