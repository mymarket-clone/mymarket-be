
using FluentValidation;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Units.Commands.Add;
public class AddUnitCommandValidator : AbstractValidator<AddUnitCommand>
{
    public AddUnitCommandValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameEn)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);

        RuleFor(x => x.NameRu)
            .MaximumLength(255).WithMessage(SharedResources.LabelLength)
            .NotEmpty().WithMessage(SharedResources.LabelRequired);
    }
}
