using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssetAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Assets.Get;
using ASM.Application.Features.Assets.List;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ASM.UnitTests.UseCases.Assets;

public sealed class GetAssetHandlerTests
{
    private readonly Mock<IReadRepository<Asset>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly GetAssetHandler _handler;

    public GetAssetHandlerTests()
    {
        _repositoryMock = new();
        _httpContextAccessorMock = new();
        _handler = new(_repositoryMock.Object, _httpContextAccessorMock.Object);
    }

    private void SetUpHttpContext()
    {
        var claims = new List<Claim> { new(nameof(Location), nameof(Location.HoChiMinh)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [Fact]
    public async Task GivenAssetId_ShouldReturnAsset_WhenAssetExists()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();
        var asset = new Asset
        {
            Id = assetId,
            Name = "Practical Soft Car",
            AssetCode = "BS000001",
            Specification = "The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design",
            InstallDate = new(2000, 1, 1),
            State = State.Available,
            Location = Location.HoChiMinh,
            CategoryId = categoryId,
        };

        SetUpHttpContext();

        var query = new GetAssetQuery(assetId);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(asset);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Id.Should().Be(assetId);
        result.Value.Name.Should().Be("Practical Soft Car");
        result.Value.AssetCode.Should().Be("BS000001");
        result.Value.Specification.Should().Be("The Apollotech B340 is an affordable wireless mouse with reliable connectivity, 12 months battery life and modern design");
        result.Value.InstallDate.Should().Be(new(2000, 1, 1));
        result.Value.State.Should().Be(State.Available);
        result.Value.Location.Should().Be(Location.HoChiMinh);
        result.Value.CategoryId.Should().Be(categoryId);

        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenAssetId_ShouldThrowNotFoundException_WhenAssetDoesNotExist()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var query = new GetAssetQuery(assetId);

        SetUpHttpContext();

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Asset?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsMissing()
    {
        // Arrange
        var query = new GetAssetQuery(Guid.NewGuid());

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsEmpty()
    {
        // Arrange
        var claims = new List<Claim> { new(nameof(AccountStatus), nameof(AccountStatus.Active)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var query = new GetAssetQuery(Guid.NewGuid());

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
