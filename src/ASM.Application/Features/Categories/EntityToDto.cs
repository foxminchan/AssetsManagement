using ASM.Application.Domain.AssetAggregate;

namespace ASM.Application.Features.Categories;

public static class EntityToDto
{
    public static CategoryDto ToCategoryDto(this Category category) =>
        new(category.Id,
            category.Name,
            category.Prefix);

    public static List<CategoryDto> ToCategoryDtos(this IEnumerable<Category> categories) =>
        categories.Select(ToCategoryDto).ToList();
}
