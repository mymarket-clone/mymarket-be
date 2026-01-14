using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Posts.Queries.GetById;

public record GetPostByIdQuery(int Id) : IRequest<PostDto?>;

public class GetPostByIdQueryHandler(
    IApplicationDbContext _context,
    IConfigurationProvider _mapper) : IRequestHandler<GetPostByIdQuery, PostDto?>
{
    public Task<PostDto?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var post = _context.Posts
            .AsNoTracking()
            .ProjectTo<PostDto>(_mapper)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        return post;
    }
}
