using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.Property(x => x.Firstname)
            .IsRequired()
            .HasColumnType("text")
            .HasMaxLength(72);

        builder.Property(x => x.LastName)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(x => x.Email)
            .HasColumnType("text")
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Gender)
            .IsRequired();

        builder.Property(x => x.BirthYear)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(x => x.PasswordHash)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.EmailVerified)
            .IsRequired();
    }
}

