using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<VerificationCodeEntity> VerificationCode { get; }
    DbSet<CategoryEntity> Categories { get; }
    DbSet<CategoryAttributesEntity> CategoryAttributes { get; }
    DbSet<CategoryBrandsEntity> CategoryBrands { get; }
    DbSet<BrandEntity> Brands { get; }
    DbSet<CityEntity> Cities { get; }
    DbSet<PostEntity> Posts { get; }
    DbSet<PostAttributesEntity> PostAttributes { get; }
    DbSet<PostsImagesEntity> PostsImages { get; }
    DbSet<PostViewEntity> PostViews { get; }
    DbSet<ImageEntity> Images { get; }
    DbSet<AttributeEntity> Attributes { get; }
    DbSet<AttributeUnitEntity> AttributeUnits { get; }
    DbSet<AttributeOptionsEntity> AttributeOptions { get; }
    DbSet<HomeCategoriesEntity> HomeCategories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);
}
