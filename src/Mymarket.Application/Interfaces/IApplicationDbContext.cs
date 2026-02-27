using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<VerificationCodeEntity> VerificationCode { get; }
    DbSet<PostEntity> Posts { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CategoryBrandsEntity> CategoryBrands { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<ImageEntity> Images { get; }
    DbSet<AttributeEntity> Attributes { get; }
    DbSet<AttributeUnitEntity> AttributeUnits { get; }
    DbSet<AttributeOptionsEntity> AttributesOptions { get; }
    DbSet<CategoryAttributesEntity> CategoryAttributes { get; }
    DbSet<PostAttributesEntity> PostAttributes{ get; }
    DbSet<BrandEntity> Brands { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}
