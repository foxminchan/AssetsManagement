using System.Security.Claims;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Features.Assignments.ListOwn;
using ASM.UnitTests.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ASM.UnitTests.UseCases.Assignments;

public sealed class ListOwnAssignmentsHandlerTest
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IReadRepository<Assignment>> _assignmentRepositoryMock;
    private readonly Mock<IReadRepository<Staff>> _staffRepositoryMock;
    private readonly ListOwnAssignmentsHandler _handler;

    public ListOwnAssignmentsHandlerTest()
    {
        _httpContextAccessorMock = new();
        _userManagerMock = new(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        _assignmentRepositoryMock = new();
        _staffRepositoryMock = new();
        _handler = new(_httpContextAccessorMock.Object, _userManagerMock.Object, _assignmentRepositoryMock.Object,
            _staffRepositoryMock.Object);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData(null, true)]
    public async Task GivenQueryRequest_ShouldReturnsAssignments_WhenAssignmentsExist(
        string? orderBy, bool isDescending)
    {
        // Arrange
        var assignments = ListAssignmentsBuilder.WithDefaultValues();

        var userId = Guid.NewGuid().ToString();
        var staffId = Guid.NewGuid();
        var user = new ApplicationUser
        {
            Id = userId, 
            StaffId = staffId
        };

        var assignedTo = new Staff
        {
            Id = staffId, 
            Users = [new()
            {
                Id = userId,
                UserName = "nhannx"
            }]
        };
        var updatedBy = new Staff { Id = Guid.NewGuid(), Users = [new()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "dientm"
        }] };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId)
        ]));

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);
        _assignmentRepositoryMock.Setup(repo =>
                repo.ListAsync(It.IsAny<AssignmentFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignments);
        _staffRepositoryMock.Setup(repo => repo.GetByIdAsync(assignments[0].StaffId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(assignedTo);
        _staffRepositoryMock.Setup(repo => repo.GetByIdAsync(assignments[0].UpdatedBy, It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedBy);

        var request = new ListOwnAssignmentsQuery(orderBy, isDescending);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);
        var enumerable = result as Assignment[] ?? result.ToArray();

        // Assert
        enumerable.Should().NotBeNull();
        enumerable.Should().HaveCount(assignments.Count);

        if (isDescending)
        {
            enumerable[0].Asset?.AssetCode.Should().Be(assignments[^1].Asset?.AssetCode);
            enumerable[^1].Asset?.AssetCode.Should().Be(assignments[0].Asset?.AssetCode);
        }
        else
        {
            enumerable[0].Asset?.AssetCode.Should().Be(assignments[0].Asset?.AssetCode);
            enumerable[^1].Asset?.AssetCode.Should().Be(assignments[^1].Asset?.AssetCode);
        }
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowsException_WhenNoUserId()
    {
        // Arrange
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = new() });

        var request = new ListOwnAssignmentsQuery(null, false);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("*userId*");
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowsException_WhenNoStaffId()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        var user = new ApplicationUser { Id = userId, StaffId = null };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId)
        ]));

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext { User = claimsPrincipal });
        _userManagerMock.Setup(x => x.FindByIdAsync(userId)).ReturnsAsync(user);

        var request = new ListOwnAssignmentsQuery(null, false);

        // Act
        Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("*staffId*");
    }
}
