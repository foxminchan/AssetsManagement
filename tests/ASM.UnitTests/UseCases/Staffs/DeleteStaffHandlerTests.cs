using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Staffs.Delete;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public sealed class DeleteStaffHandlerTests
{
    private readonly Mock<IRepository<Staff>> _repositoryMock;
    private readonly DeleteStaffHandler _handler;

    public DeleteStaffHandlerTests()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task DeleteStaffById_ShouldReturnNoContent_WhenStaffExists()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var staff = new Staff
        {
            Id = staffId,
            FirstName = "Man",
            LastName = "Vo Minh",
            Dob = new(2002, 11, 21),
            JoinedDate = new(2021, 1, 1),
            Gender = Gender.Male,
            Location = Location.HoChiMinh,
            RoleType = RoleType.Admin,
            StaffCode = "SD2002",
            IsDeleted = false,
            Users = [new ApplicationUser { Id = "user Id", UserName = "vo minh", LockoutEnd = null }]

        };
        var deleteCommand = new DeleteStaffCommand(staffId);


        _repositoryMock.Setup(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>())).ReturnsAsync(staff);

        _repositoryMock.Setup(r => r.UpdateAsync(staff, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(deleteCommand, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(staff, It.IsAny<CancellationToken>()), Times.Once);

        staff.IsDeleted.Should().Be(true);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_GivenInvalidId_ShouldThrowException()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var request = new DeleteStaffCommand(staffId);

        _repositoryMock.Setup(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Staff?)null);

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
