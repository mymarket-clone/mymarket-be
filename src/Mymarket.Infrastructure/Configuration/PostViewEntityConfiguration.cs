using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class PostViewEntityConfiguration : IEntityTypeConfiguration<PostViewEntity>
{
    public void Configure(EntityTypeBuilder<PostViewEntity> builder)
    {
        builder.ToTable("PostViews");

        builder.Property(x => x.PostId)
            .IsRequired();

        builder.Property(x => x.ViewDate)
            .IsRequired();

        builder.Property(x => x.ViewedAt)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired(false);

        builder.Property(x => x.SessionId)
            .IsRequired(false);

        builder.HasOne(x => x.Post)
            .WithMany()
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.PostId, x.ViewDate });

        builder.HasIndex(x => new { x.PostId, x.UserId, x.ViewDate });

        builder.HasIndex(x => new { x.PostId, x.SessionId, x.ViewDate });

        builder.HasIndex(x => x.ViewedAt);
    }
}
