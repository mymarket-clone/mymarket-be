using FluentValidation;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Brands.Commands.Edit;

public class EditBrandCommandValidator : AbstractValidator<EditBrandCommand>
{
    public EditBrandCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(SharedResources.NameRequired)
            .MaximumLength(255).WithMessage(SharedResources.NameMaxLength);
    }
}
