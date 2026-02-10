using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class PostAttributesEntityConfiguration : IEntityTypeConfiguration<PostAttributesEntity>
{
    public void Configure(EntityTypeBuilder<PostAttributesEntity> builder)
    {
        builder.ToTable("PostAttributes");

        builder.HasOne(x => x.Post)
               .WithMany(p => p.Attributes)
               .HasForeignKey(x => x.PostId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Attribute)
               .WithMany(a => a.PostAttributes)
               .HasForeignKey(x => x.AttributeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Option)
               .WithMany(o => o.PostAttributes)
               .HasForeignKey(x => x.OptionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.ValueText)
               .HasMaxLength(500)
               .HasColumnType("text")
               .IsUnicode(true)
               .IsRequired(false);

        builder.Property(x => x.ValueNumber)
               .IsRequired(false);
    }
}
