using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Staffs.Get;
using ASM.Application.Features.Staffs.List;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public sealed class GetStaffHandlerTests
{
    private readonly Mock<IReadRepository<Staff>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly GetStaffHandler _handler;

    public GetStaffHandlerTests()
    {
        _repositoryMock = new();
        _httpContextAccessorMock = new();
        _handler = new(_repositoryMock.Object, _httpContextAccessorMock.Object);
    }
    private void SetUpHttpContext()
    {
        var claims = new List<Claim> { new(nameof(Location), nameof(Location.HoChiMinh)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [Fact]
    public async Task GivenStaffId_ShouldReturnStaff_WhenStaffExists()
    {
        // Arrange
        var staffId = Guid.NewGuid();
        var staff = new Staff
        {
            Id = staffId,
            FirstName = "Nhan",
            LastName = "Nguyen Xuan",
            Dob = new(2000, 1, 1),
            JoinedDate = new(2021, 1, 1),
            Gender = Gender.Male,
            Location = Location.HoChiMinh,
            RoleType = RoleType.Admin,
            StaffCode = "SD2002"
        };

        SetUpHttpContext();
        
        var query = new GetStaffQuery(staffId);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(staff);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Value.Id.Should().Be(staffId);
        result.Value.FirstName.Should().Be("Nhan");
        result.Value.LastName.Should().Be("Nguyen Xuan");
        result.Value.Dob.Should().Be(new(2000, 1, 1));
        result.Value.JoinedDate.Should().Be(new(2021, 1, 1));
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenStaffId_ShouldThrowNotFoundException_WhenStaffDoesNotExist()
    {
        // Arrange
        var staffId = Guid.NewGuid();

        SetUpHttpContext();

        var query = new GetStaffQuery(staffId);

        _repositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Staff?)null);

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        _repositoryMock.Verify(r => r.FirstOrDefaultAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsMissing()
    {
        // Arrange
        var claims = new List<Claim> { new(nameof(AccountStatus), nameof(AccountStatus.Active)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var query = new GetStaffQuery(Guid.NewGuid());

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsEmpty()
    {
        // Arrange
        var claims = new List<Claim> { new(nameof(AccountStatus), nameof(AccountStatus.Active)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var query = new GetStaffQuery(Guid.NewGuid());

        // Act
        Func<Task> act = async () => await _handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
