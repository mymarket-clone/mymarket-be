using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mymarket.Domain.Entities;

namespace Mymarket.Infrastructure.Configuration;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(72);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.Gender)
            .IsRequired();

        builder.Property(x => x.BirthYear)
            .IsRequired()
            .HasMaxLength(4);

        builder.Property(x => x.PasswordHash)
            .IsRequired();

        builder.Property(x => x.EmailVerified)
            .IsRequired();
    }
}

