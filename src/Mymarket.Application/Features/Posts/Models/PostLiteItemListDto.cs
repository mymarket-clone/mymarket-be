using MediatR;

namespace Mymarket.Application.Features.Posts.Models;

public class PostLiteItemListDto
{
    public required List<PostLiteItemDto>? SuperVip { get; set; }
    public required List<PostLiteItemDto>? VipPlus { get; set; }
    public required List<PostLiteItemDto>? Vip { get; set; }
}
