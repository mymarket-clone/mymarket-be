using MediatR;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Features.Chat.Commands;

public record CreateChatCommand(
    int Reciever,
    int PostId,
    string Message
) : IRequest<Unit>;

public class CreateChatCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser) : IRequestHandler<CreateChatCommand, Unit>
{
    public async Task<Unit> Handle(CreateChatCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = (int)currentUser.Id!;

        var u1 = Math.Min(currentUserId, request.Reciever);
        var u2 = Math.Max(currentUserId, request.Reciever);

        await using var transaction = await context.BeginTransactionAsync(cancellationToken);

        try
        {
            var chat = new ChatEntity
            {
                User1Id = u1,
                User2Id = u2,
                PostId = request.PostId
            };

            await context.Chats.AddAsync(chat, cancellationToken);

            var message = new ChatMessageEntity
            {
                Chat = chat,
                SenderId = currentUserId,
                Content = request.Message
            };

            await context.ChatMessages.AddAsync(message, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Unit.Value;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}