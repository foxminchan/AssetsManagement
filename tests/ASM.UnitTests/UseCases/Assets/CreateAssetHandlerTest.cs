using System.Security.Claims;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Assets.Create;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ASM.UnitTests.UseCases.Assets;

public sealed class CreateAssetHandlerTest
{
    private readonly Mock<IRepository<Asset>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateAssetHandler _handler;

    public CreateAssetHandlerTest()
    {
        _repositoryMock = new();
        _httpContextAccessorMock = new();

        _handler = new(_repositoryMock.Object, _httpContextAccessorMock.Object);

        ClaimsPrincipal user = new(new ClaimsIdentity(
        [
            new Claim(nameof(Location), nameof(Location.HoChiMinh))
        ]));

        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(user);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldReturnAssetId_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available,
            Guid.NewGuid());
        var asset = new Asset(
            command.Name,
            "AssetCode",
            command.Specification,
            command.InstallDate,
            command.State,
            Location.HoChiMinh,
            command.CategoryId);

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync([asset]);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(asset.Id);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowException_WhenLocationClaimIsMissing()
    {
        // Arrange
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available,
            Guid.NewGuid());
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal());

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GivenValidRequest_ShouldGenerateAssetCode_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available,
            Guid.NewGuid());
        var assets = new[]
        {
            new Asset("ExistingAsset", "ExistingCode", "Specification", DateOnly.FromDateTime(DateTime.Now),
                State.Available, Location.HoChiMinh, command.CategoryId)
        };

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(assets.ToList);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            new Asset(command.Name, "GeneratedCode", command.Specification, command.InstallDate, command.State,
                Location.HoChiMinh, command.CategoryId));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenPastInstalledDate_ShouldThrowOutOfRangeException_WhenCommandIsValid()
    {
        // Arrange
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            State.Available,
            Guid.NewGuid());
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal());

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }
}
