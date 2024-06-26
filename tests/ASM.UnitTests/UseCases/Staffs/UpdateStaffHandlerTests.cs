using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Staffs.Get;
using ASM.Application.Features.Staffs.Update;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public class UpdateStaffHandlerTests
{
    private readonly Mock<IRepository<Staff>> _repositoryMock;
    private readonly UpdateStaffHandler _handler;

    private readonly DateOnly TestDob = new(2002, 5, 11);
    private readonly DateOnly TestJoinedDate = new(2023, 10, 10);
    private readonly Gender TestGender = Gender.Male;
    private readonly RoleType TestRoleType = RoleType.Staff;

    public UpdateStaffHandlerTests()
    {
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidData_ShouldInvokeUpdate_AtLeastOnce_IfUserExist()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var query = new UpdateStaffCommand(
                staffId,
                TestDob,
                TestJoinedDate,
                TestGender,
                TestRoleType);

        Staff testUser = new();

        _repositoryMock.Setup(r => r.GetByIdAsync(staffId, new CancellationToken()))
            .ReturnsAsync(testUser);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenInvalidData_ShouldThrowNotFound_IfUserNotExist()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var query = new UpdateStaffCommand(
                staffId,
                TestDob,
                TestJoinedDate,
                TestGender,
                TestRoleType);

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Staff>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetByIdAsync(staffId, It.IsAny<CancellationToken>()),
            Times.Once);

    }
}

