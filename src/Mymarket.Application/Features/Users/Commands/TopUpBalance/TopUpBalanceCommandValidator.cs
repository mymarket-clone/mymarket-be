using FluentValidation;

namespace Mymarket.Application.Features.Users.Commands.TopUpBalance;

public class TopUpBalanceCommandValidator : AbstractValidator<TopUpBalanceCommand>
{
    public TopUpBalanceCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be a positive number.");
    }
}
