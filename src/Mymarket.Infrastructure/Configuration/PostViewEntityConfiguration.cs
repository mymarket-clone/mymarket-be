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
            .WithMany(p => p.PostViews) 
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(u => u.PostViews)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade); ;

        builder.HasIndex(x => new { x.PostId, x.UserId, x.ViewDate })
            .IsUnique()
            .HasFilter("\"UserId\" IS NOT NULL");

        builder.HasIndex(x => new { x.PostId, x.SessionId, x.ViewDate })
            .IsUnique()
            .HasFilter("\"SessionId\" IS NOT NULL");

        builder.HasIndex(x => x.ViewedAt);
    }
}
