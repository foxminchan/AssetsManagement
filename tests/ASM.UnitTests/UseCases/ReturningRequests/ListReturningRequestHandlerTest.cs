using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;
using ASM.Application.Domain.ReturningRequestAggregate.Specifications;
using ASM.Application.Features.ReturningRequests.List;
using ASM.UnitTests.Builder;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Moq;

namespace ASM.UnitTests.UseCases.ReturningRequests;

public sealed class ListReturningRequestHandlerTest
{
    private readonly Mock<IReadRepository<ReturningRequest>> _returningRequestRepositoryMock;
    private readonly Mock<IReadRepository<Staff>> _staffRepositoryMock;
    private readonly ListReturningRequestHandler _handler;

    public ListReturningRequestHandlerTest()
    {
        _returningRequestRepositoryMock = new();
        _staffRepositoryMock = new();
        _handler = new(_returningRequestRepositoryMock.Object, _staffRepositoryMock.Object);
    }

    [Theory]
    [InlineData(null, null, 1, 20, nameof(Asset.AssetCode), false, null)]
    [InlineData(null, null, 1, 20, nameof(Asset.AssetCode), true, null)]
    public async Task GivenQueryRequest_ShouldReturnReturningRequests_WhenReturningRequestsExist(
        State? state, DateOnly? returnedDate, int pageIndex, int pageSize, string orderBy, bool isDescending,
        string? search)
    {
        // Arrange
        var returningRequests = ListReturningRequestsBuilder.WithDefaultValues();

        var staff1 = returningRequests[0].Staff!;
        var staff2 = returningRequests[1].Staff!;
        var staff3 = returningRequests[2].Staff!;

        var staffList = new List<Staff> { staff1, staff2, staff3 };

        _returningRequestRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<ReturningRequestFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returningRequests);
        _returningRequestRepositoryMock.Setup(repo =>
                repo.CountAsync(It.IsAny<ReturningRequestFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(returningRequests.Count);
        _staffRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(staffList);

        var request = new ListReturningRequestQuery(state, returnedDate, pageIndex, pageSize, orderBy, isDescending, search);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.PagedInfo.TotalRecords.Should().Be(returningRequests.Count);
        result.PagedInfo.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldReturnEmptyList_WhenReturningRequestsNotExist()
    {
        // Arrange
        _returningRequestRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<ReturningRequestFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _returningRequestRepositoryMock.Setup(repo =>
                repo.CountAsync(It.IsAny<ReturningRequestFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _staffRepositoryMock.Setup(repo =>
        repo.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync([]);

        var request = new ListReturningRequestQuery(null, null, 1, 10, null, false, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeEmpty();
        result.PagedInfo.TotalRecords.Should().Be(0);
        result.PagedInfo.TotalPages.Should().Be(0);
    }
}
