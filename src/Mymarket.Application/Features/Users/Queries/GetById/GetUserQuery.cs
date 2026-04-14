using MediatR;
using Mymarket.Application.Features.Users.Common.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;
using System.Data.Entity;

namespace Mymarket.Application.Features.Users.Queries.GetById;

public record GetUserByIdQuery(
    int Id
) : IRequest<UserInfoDto>;

public class GetUserByIdQueryHandler(
    IApplicationDbContext context) : IRequestHandler<GetUserByIdQuery, UserInfoDto>
{
    public async Task<UserInfoDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken)
            ?? throw new KeyNotFoundException(SharedResources.RecordNotFound);

        var userDto = new UserInfoDto(
            Id: user.Id,
            FirstName: user.Firstname,
            Lastname: user.LastName,
            Email: user.Email,
            GenderType: user.Gender,
            BirthYear: user.BirthYear,
            PhoneNumber: user.PhoneNumber,
            EmailVerified: user.EmailVerified,
            PostsCount: context.Posts.Count(p => p.UserId == user.Id)
        ); 

        return userDto;
    }
}   