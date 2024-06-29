using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Assets.Delete;
using Moq;

namespace ASM.UnitTests.UseCases.Assets;

public sealed class DeleteAssetHandlerTest
{
    private readonly Mock<IRepository<Asset>> _repositoryMock;
    private readonly DeleteAssetHandler _handler;

    public DeleteAssetHandlerTest()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccess_WhenAssetIsDeleted()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var asset = new Asset { Id = assetId };
        var request = new DeleteAssetCommand(assetId);

        _repositoryMock.Setup(r => r.GetByIdAsync(assetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(asset);
        _repositoryMock.Setup(r => r.DeleteAsync(asset, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(true);
        _repositoryMock.Verify(r => r.GetByIdAsync(assetId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(asset, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidRequest_ShouldThrowNotFound_WhenAssetDoesNotExist()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var request = new DeleteAssetCommand(assetId);

        _repositoryMock.Setup(r => r.GetByIdAsync(assetId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Asset?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetByIdAsync(assetId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
