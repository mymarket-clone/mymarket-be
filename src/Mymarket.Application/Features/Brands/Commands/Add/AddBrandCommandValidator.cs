using FluentValidation;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Brands.Commands.Add;

public class AddBrandCommandValidator : AbstractValidator<AddBrandCommand>
{
    public AddBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.NameRequired)
            .MaximumLength(255).WithMessage(SharedResources.NameMaxLength);

        RuleFor(x => x.Logo)
            .NotEmpty().WithMessage(SharedResources.ImageRequired);
    }
}
