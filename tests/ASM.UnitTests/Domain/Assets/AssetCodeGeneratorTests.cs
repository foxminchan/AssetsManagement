using ASM.Application.Domain.AssetAggregate;

namespace ASM.UnitTests.Domain.Assets;

public sealed class AssetCodeGeneratorTests
{
    [Fact]
    public void GivenCategoriesWithNoPrefix_ShouldGenerateCorrectAssetCode()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        const string prefix = "TH";
        var assets = new List<Asset>
        {
            new() { CategoryId = categoryId, AssetCode = null, Category = new() { Prefix = "TH" } }
        };

        // Act
        var newAssetCode = Asset.GenerateAssetCode(assets, prefix);

        // Assert
        newAssetCode.Should().Be("TH000001");
    }

    [Fact]
    public void GivenCategoriesWithPrefix_ShouldGenerateCorrectAssetCode_WhenNoConflict()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        const string prefix = "CT";
        var assets = new List<Asset>
        {
            new() { CategoryId = categoryId, AssetCode = "CT000001", Category = new() { Prefix = prefix } }
        };

        // Act
        var newAssetCode = Asset.GenerateAssetCode(assets, prefix);

        // Assert
        newAssetCode.Should().Be("CT000002");
    }

    [Fact]
    public void GivenCategoriesWithPrefix_ShouldGenerateCorrectAssetCode_WhenMultipleConflicts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        const string prefix = "CT";
        var assets = new List<Asset>
        {
            new() { CategoryId = categoryId, AssetCode = "CT000001", Category = new() { Prefix = prefix } },
            new() { CategoryId = categoryId, AssetCode = "CT000002", Category = new() { Prefix = prefix } }
        };

        // Act
        var newAssetCode = Asset.GenerateAssetCode(assets, prefix);

        // Assert
        newAssetCode.Should().Be("CT000003");
    }

    [Fact]
    public void GivenCategoriesWithPrefix_ShouldGenerateCorrectAssetCode_WhenMultipleFragmentedConflicts()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        const string prefix = "CT";
        var assets = new List<Asset>
        {
            new() { CategoryId = categoryId, AssetCode = "CT000001", Category = new() { Prefix = prefix } },
            new() { CategoryId = categoryId, AssetCode = "CT000003", Category = new() { Prefix = prefix } }
        };

        // Act
        var newAssetCode = Asset.GenerateAssetCode(assets, prefix);

        // Assert
        newAssetCode.Should().Be("CT000002");
    }
}
