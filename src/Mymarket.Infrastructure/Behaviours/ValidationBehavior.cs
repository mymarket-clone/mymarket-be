using FluentValidation;
using MediatR;

namespace Mymarket.Infrastructure.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> _validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if(_validators.Any())
        {
            var failures = _validators
               .Select(validator => validator.Validate(request))
               .SelectMany(result => result.Errors)
               .Where(failure => failure != null)
               .ToList();

            if (failures.Count != 0) throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}
