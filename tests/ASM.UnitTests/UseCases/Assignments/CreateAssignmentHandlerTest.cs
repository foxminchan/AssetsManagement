using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Features.Assignments.Create;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class CreateAssignmentHandlerTest
{
    private readonly Mock<IRepository<Assignment>> _repositoryMock;
    private readonly CreateAssignmentHandler _handler;

    public CreateAssignmentHandlerTest()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAssignment_ShouldReturnAssignmentId_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateAssignmentCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Now),
            "Note");

        var assignment = new Assignment(
            State.WaitingForAcceptance,
            command.AssignedDate,
            command.Note,
            command.AssetId,
            command.UserId);

        _repositoryMock.Setup(x => x.AddAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        var result = await _handler.Handle(command, It.IsAny<CancellationToken>());

        result.Value.Should().Be(assignment.Id);
        _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
