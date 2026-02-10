using Microsoft.EntityFrameworkCore;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<VerificationCodeEntity> VerificationCode { get; }
    DbSet<PostEntity> Posts { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<ImageEntity> Images { get; }
    DbSet<AttributeEntity> Attributes { get; }
    DbSet<AttributesOptionsEntity> AttributesOptions { get; }
    DbSet<CategoryAttributesEntity> CategoryAttributes { get; }
    DbSet<PostAttributesEntity> PostAttributes{ get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
