using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Categories.List;
using ASM.UnitTests.Builder;
using Moq;

namespace ASM.UnitTests.UseCases.Categories;

public class ListCategoriesHandlerTests
{
    private readonly Mock<IReadRepository<Category>> _repositoryMock;
    private readonly ListCategorieHandler _handler;

    public ListCategoriesHandlerTests()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    private static int GetTotalRecords(List<Category> categories)
    {
        return categories.Count;
    }

    [Fact]
    public async Task GivenRequest_ShouldReturnResult_WhenCategoriesIsNotEmpty()
    {
        // Arrange
        var categories = ListCategoriesBuilder.WithDefaultValues();
        int totalRecords = GetTotalRecords(categories);

        var query = new ListCategoriesQuery();

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Count().Should().Be(totalRecords);

        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
