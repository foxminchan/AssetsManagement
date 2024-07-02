using ASM.Application.Domain.AssetAggregate;

namespace ASM.UnitTests.Builder;

public static class ListCategoriesBuilder
{
    private static List<Category> _categories = [];

    public static List<Category> WithDefaultValues()
    {
        _categories =
        [
            new()
            {
                Name = "Keyboard",
                Prefix = "KB"
            },
            new()
            {
                Name = "Bluetooth Mouse",
                Prefix = "BM"
            },
            new()
            {
                Name = "Laptop",
                Prefix = "LA"
            },
            new()
            {
                Name = "Monitor",
                Prefix = "MO"
            },
            new()
            {
                Name = "Personal Computer",
                Prefix = "PC"
            },
        ];

        return _categories;
    }
}
