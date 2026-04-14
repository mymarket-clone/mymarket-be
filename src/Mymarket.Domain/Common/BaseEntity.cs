namespace Mymarket.Domain.Common;

public class BaseEntity<T> : AuditableEntity
{
    public T Id { get; set; }
}
