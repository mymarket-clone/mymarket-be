using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Attributes.Commands.Add;

public class AddAttributeCommandValidator : AbstractValidator<AddAttributeCommand>
{
    private readonly IApplicationDbContext _context;

    public AddAttributeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.Code)
            .MustAsync(AttributeCodeDoesnotExist).WithMessage(SharedResources.AttributeCodeExists)
            .MaximumLength(255).WithMessage(SharedResources.AttributeCodeLength)
            .NotEmpty().WithMessage(SharedResources.AttributeCodeRequired);

        RuleFor(x => x.AttributeType)
            .IsInEnum().WithMessage(SharedResources.AttributeTypeInvalid)
            .NotEmpty().WithMessage(SharedResources.AttributeTypeRequired);
    }

    private async Task<bool> AttributeCodeDoesnotExist(string code, CancellationToken cancellationToken)
    {
        return await _context.Attributes.AnyAsync(c => c.Code == code, cancellationToken);
    }
}
