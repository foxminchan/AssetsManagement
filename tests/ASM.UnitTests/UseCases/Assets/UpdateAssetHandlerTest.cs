using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Assets.Update;
using Moq;

namespace ASM.UnitTests.UseCases.Assets;

public sealed class UpdateAssetHandlerTest
{
    private readonly Mock<IRepository<Asset>> _repositoryMock;
    private readonly UpdateAssetHandler _handler;

    public UpdateAssetHandlerTest()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccessResult_WhenCommandIsValid()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var command = new UpdateAssetCommand(
            assetId,
            "UpdatedName",
            "UpdatedSpecification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available);
        var asset = new Asset(
            command.Name,
            "AssetCode",
            command.Specification,
            command.InstallDate,
            command.State,
            Location.HoChiMinh,
            Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(asset);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(true);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var command = new UpdateAssetCommand(
            assetId,
            "UpdatedName",
            "UpdatedSpecification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available);

        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Asset?)null);

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
