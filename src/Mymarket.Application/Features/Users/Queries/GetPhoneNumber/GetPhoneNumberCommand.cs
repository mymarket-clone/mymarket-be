using MediatR;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Users.Queries.GetPhoneNumber;

public record GetPhoneNumberCommand(int Id) : IRequest<string?>;

public class GetPhoneNumberCommandHandler(
    IApplicationDbContext context) : IRequestHandler<GetPhoneNumberCommand, string?>
{
    public Task<string?> Handle(GetPhoneNumberCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = context.Users
            .Where(u => u.Id == request.Id)
            .Select(u => u.PhoneNumber)
            .FirstOrDefault();

        return Task.FromResult(phoneNumber);
    }
}