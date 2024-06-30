using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Features.Assignments.List;
using ASM.UnitTests.Builder;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class ListAssignmentsHandlerTest
{
    private readonly Mock<IReadRepository<Staff>> _staffRepositoryMock;
    private readonly Mock<IReadRepository<Assignment>> _assignmentRepositoryMock;
    private readonly ListAssignmentsHandler _handler;

    public ListAssignmentsHandlerTest()
    {
        _staffRepositoryMock = new();
        _assignmentRepositoryMock = new();
        _handler = new(_assignmentRepositoryMock.Object, _staffRepositoryMock.Object);
    }

    [Theory]
    [InlineData(null, null, 1, 20, nameof(Asset.AssetCode), false, null)]
    [InlineData(null, null, 1, 20, nameof(Asset.AssetCode), true, null)]
    public async Task GivenQueryRequest_ShouldReturnsAssignments_WhenAssignmentsExist(
        State? state, DateOnly? assignedDate, int pageIndex, int pageSize, string orderBy, bool isDescending,
        string? search)
    {
        // Arrange
        var assignments = ListAssignmentsBuilder.WithDefaultValues();

        var staff1 = new Staff
        {
            Id = assignments[0].StaffId,
            Users = [new() { UserName = "nhannx" }]
        };
        var staff2 = new Staff
        {
            Id = assignments[1].StaffId,
            Users = [new() { UserName = "dientm" }]
        };
        var updatedBy1 = new Staff
        {
            Id = assignments[0].UpdatedBy,
            Users = [new() { UserName = "minhnl" }]
        };
        var updatedBy2 = new Staff
        {
            Id = assignments[1].UpdatedBy,
            Users = [new() { UserName = "nhannx" }]
        };

        var staffList = new List<Staff> { staff1, staff2, updatedBy1, updatedBy2 };

        _assignmentRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments);
        _assignmentRepositoryMock.Setup(repo =>
                repo.CountAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments.Count);
        _staffRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(staffList);

        var request = new ListAssignmentsQuery(state, assignedDate, pageIndex, pageSize, orderBy, isDescending, search);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        if (isDescending)
        {
            result.Value.First().Asset?.AssetCode.Should().Be(assignments[^1].Asset?.AssetCode);
            result.Value.Last().Asset?.AssetCode.Should().Be(assignments[0].Asset?.AssetCode);
        }
        else
        {
            result.Value.First().Asset?.AssetCode.Should().Be(assignments[0].Asset?.AssetCode);
            result.Value.Last().Asset?.AssetCode.Should().Be(assignments[^1].Asset?.AssetCode);
        }

        result.PagedInfo.TotalRecords.Should().Be(assignments.Count);
        result.PagedInfo.TotalPages.Should().Be(1);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldReturnsAssignments_WhenAssignmentsNotExist()
    {
        // Arrange
        _assignmentRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _assignmentRepositoryMock.Setup(repo =>
                repo.CountAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(0);
        _staffRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var request = new ListAssignmentsQuery(State.Accepted, DateOnly.FromDateTime(DateTime.Now), 1, 20, nameof(Asset.AssetCode), false, null);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeEmpty();
        result.PagedInfo.TotalRecords.Should().Be(0);
        result.PagedInfo.TotalPages.Should().Be(0);
    }
}
