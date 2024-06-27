using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Features.Assignments.Delete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class DeleteAssignmentHandlerTest
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IRepository<Assignment>> _repositoryMock;
    private readonly DeleteAssignmentHandler _handler;
    private readonly ClaimsPrincipal _userClaimsPrincipal;
    private readonly ClaimsPrincipal _adminClaimsPrincipal;

    public DeleteAssignmentHandlerTest()
    {
        _httpContextAccessorMock = new();
        _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _repositoryMock = new();
        _handler = new(_httpContextAccessorMock.Object, _userManagerMock.Object, _repositoryMock.Object);

        _userClaimsPrincipal = new(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(nameof(AuthRole), AuthRole.User)
        ]));

        _adminClaimsPrincipal = new(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(nameof(AuthRole), AuthRole.Admin)
        ]));
    }

    [Fact]
    public async Task GivenAdminRequest_ShouldReturnSuccess_WhenDeleteValid()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());
        var assignment = new Assignment { Id = command.Id };

        _httpContextAccessorMock.Setup(h => h.HttpContext!.User).Returns(_adminClaimsPrincipal);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        _repositoryMock.Setup(r => r.DeleteAsync(assignment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(assignment, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenUserRequest_ShouldReturnSuccess_WhenDeleteValid()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());
        var assignment = new Assignment { Id = command.Id };
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString(), StaffId = Guid.NewGuid() };

        _httpContextAccessorMock.Setup(h => h.HttpContext!.User).Returns(_userClaimsPrincipal);

        _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        _repositoryMock.Setup(r => r.DeleteAsync(assignment, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(assignment, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenInvalidRequest_ShouldReturnNotFound_WhenAssignmentDoesNotExist()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());

        _httpContextAccessorMock.Setup(h => h.HttpContext!.User).Returns(_adminClaimsPrincipal);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Assignment?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GivenUserRequest_ShouldThrow_WhenUserIdIsNull()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());
        _httpContextAccessorMock.Setup(h => h.HttpContext!.User).Returns(new ClaimsPrincipal());

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _userManagerMock.Verify(u => u.FindByIdAsync(It.IsAny<string>()), Times.Never);
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GivenUserRequest_ShouldThrow_WhenStaffIdIsNull()
    {
        // Arrange
        var command = new DeleteAssignmentCommand(Guid.NewGuid());
        var user = new ApplicationUser { Id = Guid.NewGuid().ToString() };

        _httpContextAccessorMock.Setup(h => h.HttpContext!.User).Returns(_userClaimsPrincipal);
        _userManagerMock.Setup(u => u.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Assignment>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
