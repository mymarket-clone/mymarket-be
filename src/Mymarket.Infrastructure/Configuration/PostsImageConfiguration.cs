using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class PostsImageConfiguration : IEntityTypeConfiguration<PostsImagesEntity>
{
    public void Configure(EntityTypeBuilder<PostsImagesEntity> builder)
    {
        builder.ToTable("PostsImages");

        builder
            .HasOne(x => x.Post)
            .WithMany(x => x.PostsImages)
            .HasForeignKey(x => x.PostId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Image)
            .WithMany(i => i.PostsImages)
            .HasForeignKey(x => x.ImageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(x => x.Order)
            .IsRequired();
    }
}
