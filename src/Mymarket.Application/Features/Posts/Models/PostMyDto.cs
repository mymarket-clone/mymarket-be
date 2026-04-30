using Mymarket.Domain.Common;

namespace Mymarket.Application.Features.Posts.Models;

public class PostMyDto
{
    public required PaginatedResult<PostMyItemDto> Result { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public int BlockedCount { get; set; }
    public int TotalViews { get; set; }
}
