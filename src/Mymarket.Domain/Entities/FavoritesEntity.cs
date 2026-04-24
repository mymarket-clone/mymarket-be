namespace Mymarket.Domain.Entities;

public class FavoritesEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public UserEntity? User { get; set; }
    public PostEntity? Post { get; set; }
}
