using Mymarket.Domain.Common;

namespace Mymarket.Domain.Entities;

public class CityEntity : BaseEntity<int>
{
   public required string Name { get; set; }
   public ICollection<PostEntity>? Posts { get; set; }
}
