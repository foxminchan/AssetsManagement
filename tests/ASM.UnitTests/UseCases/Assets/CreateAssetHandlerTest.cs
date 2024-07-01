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
    private readonly Mock<IRepository<Asset>> _assetRepositoryMock;
    private readonly Mock<IRepository<Category>> _categoryRepositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateAssetHandler _handler;

    public CreateAssetHandlerTest()
    {
        _assetRepositoryMock = new();
        _categoryRepositoryMock = new();
        _httpContextAccessorMock = new();

        _handler = new(_assetRepositoryMock.Object, _categoryRepositoryMock.Object, _httpContextAccessorMock.Object);

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
        var categoryId = Guid.NewGuid();
        var category = new Category
        {
            Id = categoryId,
            Name = "CategoryName",
            Prefix = "CategoryPrefix",
        };
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available,
            categoryId);
        var asset = new Asset(
            command.Name,
            "AssetCode",
            command.Specification,
            command.InstallDate,
            command.State,
            Location.HoChiMinh,
            command.CategoryId);

        _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync([asset]);
        _assetRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).ReturnsAsync(asset);
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Value.Should().Be(asset.Id);
        _assetRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>()), Times.Once);
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
        var categoryId = Guid.NewGuid();
        var category = new Category
        {
            Id = categoryId,
            Name = "CategoryName",
            Prefix = "CategoryPrefix",
        };
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now),
            State.Available,
            categoryId);
        var assets = new[]
        {
            new Asset("ExistingAsset", "ExistingCode", "Specification", DateOnly.FromDateTime(DateTime.Now),
                State.Available, Location.HoChiMinh, command.CategoryId)
        };

        _assetRepositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>())).ReturnsAsync(assets.ToList);
        _assetRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Asset>(), It.IsAny<CancellationToken>())).ReturnsAsync(
            new Asset(command.Name, "GeneratedCode", command.Specification, command.InstallDate, command.State,
                Location.HoChiMinh, command.CategoryId));
        _categoryRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(category);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _assetRepositoryMock.Verify(r => r.ListAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task GivenPastInstalledDate_ShouldThrowOutOfRangeException_WhenCommandIsValid()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new CreateAssetCommand(
            "AssetName",
            "Specification",
            DateOnly.FromDateTime(DateTime.Now.AddDays(-1)),
            State.Available,
            categoryId);
        _httpContextAccessorMock.Setup(x => x.HttpContext!.User).Returns(new ClaimsPrincipal());

        // Act
        Func<Task> action = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await action.Should().ThrowAsync<ArgumentException>();
    }
}
