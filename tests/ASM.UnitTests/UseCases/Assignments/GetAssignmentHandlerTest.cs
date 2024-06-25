using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Features.Assignments.Get;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class GetAssignmentHandlerTest
{
    private readonly Mock<IReadRepository<Staff>> _staffRepositoryMock;
    private readonly Mock<IReadRepository<Assignment>> _assignmentRepositoryMock;
    private readonly GetAssignmentHandler _handler;

    public GetAssignmentHandlerTest()
    {
        _staffRepositoryMock = new();
        _assignmentRepositoryMock = new();
        _handler = new(_assignmentRepositoryMock.Object, _staffRepositoryMock.Object);
    }

    [Theory]
    [InlineData("nhannx", "nhannx")]
    [InlineData("nhannx", "dientm")]
    public async Task GivenAssignmentId_ShouldBeReturnsAssignment_WhenAssignmentExists(string assignedToUserName,
        string assignedByUserName)
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var staffId = Guid.NewGuid();
        var updatedById = Guid.NewGuid();

        var assignment = new Assignment { Id = assignmentId, StaffId = staffId, UpdatedBy = updatedById };

        var assignedTo = new Staff { Id = staffId, Users = [new() { UserName = assignedToUserName }] };

        var assignedBy = new Staff { Id = updatedById, Users = [new() { UserName = assignedByUserName }] };

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);
        _staffRepositoryMock.Setup(repo => repo.GetByIdAsync(staffId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignedTo);
        _staffRepositoryMock.Setup(repo => repo.GetByIdAsync(updatedById, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignedBy);

        var request = new GetAssignmentQuery(assignmentId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(assignment);
        result.Value.AssignedTo.Should().Be(assignedToUserName);
        result.Value.AssignedBy.Should().Be(assignedByUserName);
    }

    [Fact]
    public async Task GivenAssignmentId_ShouldBeReturnsNotFound_WhenAssignmentExists()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();

        _assignmentRepositoryMock.Setup(repo => repo.GetByIdAsync(assignmentId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Assignment?)null);

        var request = new GetAssignmentQuery(assignmentId);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
