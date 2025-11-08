namespace Mymarket.Domain.Common;

public class BaseEntity<T>
{
    public required T Id {get; set; }
}
