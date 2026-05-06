using MediatR;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Chat.Commands.SendMessage;

public record SendMessageCommand(
    int ChatId,
    string Content
) : IRequest<Unit>;

public class SendMessageCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IChatNotifier chatNotifier) : IRequestHandler<SendMessageCommand, Unit>
{
    public async Task<Unit> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var userId = (int)currentUser.Id!;

        var chat = await context.Chats
            .FirstOrDefaultAsync(x => x.Id == request.ChatId, cancellationToken);

        if (chat is null)
            throw new Exception("Chat not found");

        var message = new ChatMessageEntity
        {
            ChatId = request.ChatId,
            SenderId = userId,
            Content = request.Content
        };

        context.ChatMessages.Add(message);
        await context.SaveChangesAsync(cancellationToken);

        await chatNotifier.SendMessage(
            request.ChatId.ToString(),
            new
            {
                message.Id,
                message.ChatId,
                message.SenderId,
                message.Content
            }
        );

        return Unit.Value;
    }
}
