using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Features.Assignments.GetOwn;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class GetOwnAssignmentHandlerTest
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IReadRepository<Assignment>> _assignmentRepositoryMock;
    private readonly Mock<IReadRepository<Staff>> _staffRepositoryMock;
    private readonly GetOwnAssignmentHandler _handler;

    public GetOwnAssignmentHandlerTest()
    {
        _httpContextAccessorMock = new();
        _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _assignmentRepositoryMock = new();
        _staffRepositoryMock = new();
        _handler = new(_httpContextAccessorMock.Object, _userManagerMock.Object, _assignmentRepositoryMock.Object,
            _staffRepositoryMock.Object);
    }

    [Fact]
    public async Task GivenValidRequest_ShouldReturnAssignment_WhenDataIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var staffId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new(new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]))
        };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var user = new ApplicationUser { Id = userId, StaffId = staffId };
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

        var assignment = new Assignment { Id = assignmentId, StaffId = staffId, UpdatedBy = staffId };
        _assignmentRepositoryMock.Setup(x =>
                x.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignment);

        var staff = new Staff { Id = staffId, Users = [new() { Id = userId, UserName = "nhannx" }] };
        _staffRepositoryMock.Setup(x => x.GetByIdAsync(staffId, It.IsAny<CancellationToken>())).ReturnsAsync(staff);

        // Act
        var result = await _handler.Handle(new(assignmentId), CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Value.Id.Should().Be(assignmentId);
        result.Value.StaffId.Should().Be(staffId);
        result.Value.AssignedTo.Should().Be("nhannx");
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowsException_WhenUserIdIsNull()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        // Act
        Func<Task> act = async () => await _handler.Handle(new(Guid.NewGuid()), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowsException_WhenNoUserId()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var httpContext = new DefaultHttpContext
        {
            User = new(new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]))
        };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        _userManagerMock.Setup(x => x.FindByIdAsync(userId))
            .ReturnsAsync(new ApplicationUser { Id = userId, StaffId = null });

        // Act
        Func<Task> act = async () => await _handler.Handle(new(Guid.NewGuid()), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowsException_WhenAssignmentNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var staffId = Guid.NewGuid();
        var assignmentId = Guid.NewGuid();
        var httpContext = new DefaultHttpContext
        {
            User = new(new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userId)
            ]))
        };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var user = new ApplicationUser { Id = userId, StaffId = staffId };
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

        _assignmentRepositoryMock.Setup(x =>
                x.FirstOrDefaultAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Assignment?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(new(assignmentId), CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}
