using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mymarket.Application.Interfaces;
using Mymarket.Application.Resources;

namespace Mymarket.Application.Features.Chat.Commands;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUser _currentUser;

    public CreateChatCommandValidator(
        IApplicationDbContext context,
        ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;

        RuleFor(x => x.Reciever)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .GreaterThan(0).WithMessage(SharedResources.PositiveValueRequired)
            .MustAsync(UserExists)
            .WithMessage(SharedResources.RecordNotFound);

        RuleFor(x => x.PostId)
            .NotEmpty().WithMessage(SharedResources.IdRequired)
            .GreaterThan(0).WithMessage(SharedResources.PositiveValueRequired)
            .MustAsync(PostExists)
            .WithMessage(SharedResources.RecordNotFound);

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .Must(m => !string.IsNullOrWhiteSpace(m))
            .WithMessage("Message cannot be empty.");

        RuleFor(x => x)
            .MustAsync(NotAlreadyInChat)
            .WithMessage(SharedResources.ChatAlreadyExists);
    }

    private async Task<bool> UserExists(int userId, CancellationToken ct)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, ct);
    }

    private async Task<bool> PostExists(int postId, CancellationToken ct)
    {
        return await _context.Posts.AnyAsync(p => p.Id == postId, ct);
    }

    private async Task<bool> NotAlreadyInChat(CreateChatCommand command, CancellationToken ct)
    {
        var currentUserId = _currentUser.Id
            ?? throw new UnauthorizedAccessException();

        var u1 = Math.Min(currentUserId, command.Reciever);
        var u2 = Math.Max(currentUserId, command.Reciever);

        return !await _context.Chats.AnyAsync(x =>
            x.User1Id == u1 &&
            x.User2Id == u2 &&
            x.PostId == command.PostId,
            ct);
    }
}