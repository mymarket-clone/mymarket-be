using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.ToTable("Posts");

        builder
            .Property(x => x.Title)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(x => x.TitleEn)
            .HasColumnType("text")
            .HasMaxLength(255);

        builder
            .Property(x => x.TitleRu)
            .HasColumnType("text")
            .HasMaxLength(255);

        builder
            .Property(x => x.Description)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(4000);

        builder
            .Property(x => x.DescriptionEn)
            .HasColumnType("text")
            .HasMaxLength(4000);

        builder
            .Property(x => x.DescriptionRu)
            .HasColumnType("text")
            .HasMaxLength(4000);

        builder
            .Property(x => x.Name)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(72);

        builder
            .Property(x => x.PhoneNumber)
            .IsRequired();

        builder
            .HasOne(p => p.City)
            .WithMany(c => c.Posts)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
