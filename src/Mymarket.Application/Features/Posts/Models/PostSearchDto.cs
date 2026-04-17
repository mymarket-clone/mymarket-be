using Mymarket.Domain.Common;

namespace Mymarket.Application.Features.Posts.Models;

public class PostSearchDto
{
    public PaginatedResult<PostDto> Result { get; set; } = new();
    public List<CategoryBreadcrumbDto>? Breadcrumb { get; set; } = [];
    public List<CategoryLiteDto> Categories { get; set; } = [];
}
