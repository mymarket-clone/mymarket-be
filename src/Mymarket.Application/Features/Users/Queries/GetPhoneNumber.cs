using MediatR;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Users.Queries;

public record GetPhoneNumber(int Id) : IRequest<string?>;

public class GetPhoneNumberHandler(
    IApplicationDbContext context) : IRequestHandler<GetPhoneNumber, string?>
{
    public Task<string?> Handle(GetPhoneNumber request, CancellationToken cancellationToken)
    {
        var phoneNumber = context.Users
            .Where(u => u.Id == request.Id)
            .Select(u => u.PhoneNumber)
            .FirstOrDefault();

        return Task.FromResult(phoneNumber);
    }
}