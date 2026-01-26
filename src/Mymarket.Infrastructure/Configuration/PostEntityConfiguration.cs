using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

internal class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.ToTable("Posts");

        builder
            .Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder
            .Property(x => x.TitleEn)
            .HasMaxLength(255);

        builder
            .Property(x => x.TitleRu)
            .HasMaxLength(255);

        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(4000);

        builder
            .Property(x => x.DescriptionEn)
            .HasMaxLength(4000);

        builder
            .Property(x => x.DescriptionRu)
            .HasMaxLength(4000);

        builder
            .Property(x => x.Name)
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
