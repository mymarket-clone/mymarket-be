using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;

namespace Mymarket.Application.Features.Chat.Query.ChatExists;

public record ChatExistsQuery(
    int Reciever
) : IRequest<bool>;

public class ChatExistsQueryHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<ChatExistsQuery, bool>
{
    public async Task<bool> Handle(ChatExistsQuery request, CancellationToken cancellationToken)
    {
        var currentUserId = (int)currentUser.Id!;

        var u1 = Math.Min(currentUserId, request.Reciever);
        var u2 = Math.Max(currentUserId, request.Reciever);

        var chatExists = await context.Chats.AnyAsync(x => x.User1Id == u1 &&  x.User2Id == u2, cancellationToken);

        return chatExists;
    }
}
