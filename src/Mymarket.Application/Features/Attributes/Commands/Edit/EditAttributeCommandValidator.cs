using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Attributes.Commands.Edit;

public class EditAttributeCommandValidator : AbstractValidator<EditAttributeCommand>
{
    private readonly IApplicationDbContext _context;

    public EditAttributeCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(SharedResources.AttributeIdRequired)
            .MustAsync(AttributeExists).WithMessage(SharedResources.AttributeDoesnotExist);

        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength);

        RuleFor(x => x.Code)
            .MaximumLength(255).WithMessage(SharedResources.AttributeCodeLength)
            .NotEmpty().WithMessage(SharedResources.AttributeCodeRequired)
            .MustAsync((command, code, cancellationToken) => AttributeCodeDoesnotExist(code, command.Id, cancellationToken))
            .WithMessage(SharedResources.AttributeCodeExists);


        RuleFor(x => x.AttributeType)
            .IsInEnum().WithMessage(SharedResources.AttributeTypeInvalid)
            .NotEmpty().WithMessage(SharedResources.AttributeTypeRequired);
    }

    private async Task<bool> AttributeCodeDoesnotExist(string code, int id,CancellationToken cancellationToken)
    {
        return !await _context.Attributes
            .AnyAsync(c => c.Code == code && c.Id != id, cancellationToken);
    }

    private async Task<bool> AttributeExists(int id, CancellationToken cancellationToken)
    {
        return await _context.Attributes.AnyAsync(c => c.Id == id, cancellationToken);
    }
}


