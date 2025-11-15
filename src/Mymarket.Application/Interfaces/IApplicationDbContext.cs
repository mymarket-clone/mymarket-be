using Microsoft.EntityFrameworkCore;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<UserEntity> Users { get; }
    DbSet<VerificationCode> VerificationCode { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
