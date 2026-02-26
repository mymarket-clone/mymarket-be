using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class PostAttributesEntityConfiguration : IEntityTypeConfiguration<PostAttributesEntity>
{
    public void Configure(EntityTypeBuilder<PostAttributesEntity> builder)
    {
        builder.ToTable("PostAttributes");

        builder
            .HasOne(x => x.Attribute)
            .WithMany(a => a.PostAttributes)
            .HasForeignKey(x => x.AttributeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .Property(x => x.Value)
            .HasMaxLength(500)
            .HasColumnType("text")
            .IsUnicode(true)
            .IsRequired(false);

        builder
            .Property(x => x.ValueType)
            .IsRequired(true);
    }
}
