using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.AttributeOptions.Commands.Edit;

public record EditAttributeOptionCommand(
    int Id,
    int Order,
    string Name,
    string? NameEn,
    string? NameRu
) : IRequest;

public class EditAttributeOptionCommandHandler(
    IApplicationDbContext context) : IRequestHandler<EditAttributeOptionCommand>
{
    public async Task Handle(EditAttributeOptionCommand request, CancellationToken cancellationToken)
    {
        var attributeOption = await context.AttributeOptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        attributeOption!.Order = request.Order;
        attributeOption!.Name = request.Name;
        attributeOption!.NameEn = request.NameEn;
        attributeOption!.NameRu = request.NameRu;

        await context.SaveChangesAsync(cancellationToken);
    }
}