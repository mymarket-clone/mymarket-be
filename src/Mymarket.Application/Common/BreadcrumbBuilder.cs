using Mymarket.Application.Features.Posts.Models;
using Mymarket.Application.Interfaces;
using Mymarket.Domain.Entities;

namespace Mymarket.Application.Common;

public static class BreadcrumbBuilder
{
    public static List<CategoryBreadcrumbDto> Build(
        int categoryId,
        List<CategoryEntity> categories,
        ILanguageContext languageContext)
    {
        var result = new List<CategoryBreadcrumbDto>();
        var lookup = categories.ToDictionary(x => x.Id);
        var currentId = categoryId;

        while (lookup.TryGetValue(currentId, out var category))
        {
            var name = languageContext.LocalizeProperty<CategoryEntity>("Name")(category);

            result.Add(new CategoryBreadcrumbDto
            {
                Id = category.Id,
                Name = name!,
                HasChildren = categories.Any(c => c.ParentId == category.Id)
            });

            if (category.ParentId == null)
                break;

            currentId = category.ParentId.Value;
        }

        result.Reverse();
        return result;
    }
}
