using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Features.Assignments.Update;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public class UpdateAssignmentHandlerTest
{
    private readonly Mock<IRepository<Assignment>> _repositoryMock;
    private readonly UpdateAssignmentHandler _handler;

    public UpdateAssignmentHandlerTest()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenInValidData_ShouldThrowNotFound_IfUserNotExist()
    {
        // Arrange
        var assignmentId = Guid.NewGuid();
        var query = new UpdateAssignmentCommand(
                 assignmentId,
                 Guid.NewGuid(),
                 Guid.NewGuid(),
                 DateOnly.FromDateTime(DateTime.Now),
                 "Note");

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetByIdAsync(assignmentId, It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
