using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Features.Chat.Models;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Chat.Query.MyChatsQuery;

public record MyChatsQuery : IRequest<List<ChatDto>>;

public class MyChatsQueryHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<MyChatsQuery, List<ChatDto>>
{
    public async Task<List<ChatDto>> Handle(MyChatsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = (int)currentUser.Id!;

        var chats = await context.Chats
            .Where(c => c.User1Id == currentUserId || c.User2Id == currentUserId)
            .Select(c => new ChatDto
            {
                Id = c.Id,
                Name = c.User1Id == currentUserId
                    ? c.User2.Firstname : c.User1.Firstname,
                LastMessage = context.ChatMessages
                    .Where(m => m.ChatId == c.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => m.Content)
                    .FirstOrDefault()!,
                LastMessageDate = context.ChatMessages
                    .Where(m => m.ChatId == c.Id)
                    .OrderByDescending(m => m.CreatedAt)
                    .Select(m => DateOnly.FromDateTime(m.CreatedAt))
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        return chats;
    }
}
