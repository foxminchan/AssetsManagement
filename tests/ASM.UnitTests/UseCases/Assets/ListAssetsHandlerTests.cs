using System.Security.Claims;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssetAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Assets.List;
using ASM.UnitTests.Builder;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ASM.UnitTests.UseCases.Assets;

public class ListAssetsHandlerTests
{
    private readonly Mock<IReadRepository<Asset>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ListAssetsHandler _handler;

    public ListAssetsHandlerTests()
    {
        _repositoryMock = new();
        _httpContextAccessorMock = new();
        _handler = new(_repositoryMock.Object, _httpContextAccessorMock.Object);
    }

    private static int GetTotalRecords(List<Asset> assets, State[]? state, string? search)
    {
        if (!string.IsNullOrEmpty(search) && state!.Length != 0)
        {
            return assets.Count(x => x.AssetCode == search && state.Contains(x.State));
        }
        if (!string.IsNullOrEmpty(search))
        {
            return assets.Count(x => x.AssetCode == search);
        }
        return state!.Length != 9
            ? assets.Count(x => state.Contains(x.State))
            : assets.Count;
    }

    private void SetUpHttpContext()
    {
        var claims = new List<Claim> { new(nameof(Location), nameof(Location.HoChiMinh)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [Theory]
    [InlineData(new State[] { State.Recycled, State.WaitingForRecycling }, 1, 20, nameof(Asset.AssetCode), false, null)]
    [InlineData(new State[] { State.Assigned, State.Available }, 1, 20, nameof(Asset.AssetCode), true, null)]
    [InlineData(new State[] { State.Assigned, State.Available }, 1, 20, nameof(Asset.AssetCode), false, "MC000008")]
    [InlineData(new State[] { State.Recycled, State.Assigned }, 1, 20, nameof(Asset.AssetCode), true, null)]
    [InlineData(new State[] { State.WaitingForRecycling, State.Available }, 1, 20, nameof(Asset.AssetCode), true, null)]
    public async Task GivenQueryRequest_ShouldReturnPagedResult_WhenAssetsExist(
        State[] state, int pageIndex, int pageSize, string orderBy, bool isDescending, string? search)
    {
        // Arrange
        var assets = ListAssetsBuilder.WithDefaultValues();
        int totalRecords = GetTotalRecords(assets, state, search);

        SetUpHttpContext();

        var query = new ListAssetsQuery(null, state, pageIndex, pageSize, orderBy, isDescending, search);

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assets);
        _repositoryMock.Setup(r => r.CountAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalRecords);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.PagedInfo.TotalRecords.Should().Be(totalRecords);
        result.PagedInfo.TotalPages.Should().Be((int)Math.Ceiling((double)totalRecords / pageSize));

        if (!string.IsNullOrEmpty(search))
        {
            result.Value.Should().Contain(x => x.AssetCode == search);
        }

        var firstAsset = result.Value.First();
        var lastAsset = result.Value.Last();

        if (orderBy == nameof(Asset.AssetCode))
        {
            firstAsset.AssetCode.Should().Be(assets[0].AssetCode);
            lastAsset.AssetCode.Should().Be(assets[^1].AssetCode);
        }
        else
        {
            firstAsset.AssetCode.Should().Be(assets[^1].AssetCode);
            lastAsset.AssetCode.Should().Be(assets[0].AssetCode);
        }

        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<AssetFilterSpec>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsMissing()
    {
        // Arrange
        var query = new ListAssetsQuery(null, null, 1, 10, null, false, null);

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

        var query = new ListAssetsQuery(null, null, 1, 10, null, false, null);

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
