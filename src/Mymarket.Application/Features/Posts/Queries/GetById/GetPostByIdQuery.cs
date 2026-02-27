using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Posts.Queries.GetById;

public record GetPostByIdQuery(int Id) : IRequest<PostDto?>;

public class GetPostByIdQueryHandler(
    IApplicationDbContext context,
    IMapper mapper) : IRequestHandler<GetPostByIdQuery, PostDto?>
{
    public async Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        return await context.Posts
            .AsNoTracking()
            .ProjectTo<PostDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}
