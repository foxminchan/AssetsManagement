using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Categories.Create;
using Moq;

namespace ASM.UnitTests.UseCases.Categories;

public class CreateCategoryHandlerTest
{
    private readonly Mock<IRepository<Category>> _repositoryMock;
    private readonly CreateCategoryHandler _handler;

    public CreateCategoryHandlerTest()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldReturnCategoryId_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateCategoryCommand("CategoryName", "CAT");
        var category = new Category(command.Name, command.Prefix);
        var expectedResult = Result.Success(category.Id);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(expectedResult.Value);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowException_WhenCategoryAlreadyExists()
    {
        // Arrange
        var command = new CreateCategoryCommand("CategoryName", "CT");

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Category already exists"));

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenEmptyName_ShouldThrowException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateCategoryCommand("", "CT");

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GivenEmptyPrefix_ShouldThrowException_WhenCommandIsInvalid()
    {
        // Arrange
        var command = new CreateCategoryCommand("CategoryName", "");

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }
}
