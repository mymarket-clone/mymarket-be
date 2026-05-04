using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class ChatEntityConfiguration : IEntityTypeConfiguration<ChatEntity>
{
    public void Configure(EntityTypeBuilder<ChatEntity> builder)
    {
        builder.ToTable("Chats");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Post)
            .WithMany()
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User1)
            .WithMany()
            .HasForeignKey(x => x.User1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User2)
            .WithMany()
            .HasForeignKey(x => x.User2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Messages)
            .WithOne(x => x.Chat)
            .HasForeignKey(x => x.ChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.User1Id, x.User2Id, x.PostId })
            .IsUnique();
    }
}
