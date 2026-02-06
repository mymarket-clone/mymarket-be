namespace Mymarket.Domain.Common;

public class BaseEntity<T> where T : notnull
{
    public T Id {get; set; }
}
