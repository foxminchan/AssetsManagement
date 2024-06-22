using System.Security.Claims;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Features.Staffs.List;
using ASM.UnitTests.Builder;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public class ListStaffHandlerTests
{
    private readonly Mock<IReadRepository<Staff>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly ListStaffHandler _handler;

    public ListStaffHandlerTests()
    {
        _repositoryMock = new();
        _httpContextAccessorMock = new();
        _handler = new(_repositoryMock.Object, _httpContextAccessorMock.Object);
    }

    private static int GetTotalRecords(List<Staff> staffs, RoleType? roleType, string? search)
    {
        if (!string.IsNullOrEmpty(search) && roleType.HasValue)
        {
            return staffs.Count(x => x.FirstName == search && x.RoleType == roleType);
        }
        if (!string.IsNullOrEmpty(search))
        {
            return staffs.Count(x => x.FirstName == search);
        }
        return roleType.HasValue 
            ? staffs.Count(x => x.RoleType == roleType) 
            : staffs.Count;
    }

    private void SetUpHttpContext()
    {
        var claims = new List<Claim> { new(nameof(Location), nameof(Location.HoChiMinh)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
    }

    [Theory]
    [InlineData(RoleType.Admin, 1, 20, nameof(Staff.StaffCode), false, null)]
    [InlineData(RoleType.Admin, 1, 20, nameof(Staff.StaffCode), true, null)]
    [InlineData(RoleType.Admin, 1, 20, nameof(Staff.StaffCode), false, "Nhan")]
    [InlineData(null, 1, 20, nameof(Staff.FullName), true, null)]
    public async Task GivenQueryRequest_ShouldReturnPagedResult_WhenStaffsExist(
        RoleType? roleType, int pageIndex, int pageSize, string orderBy, bool isDescending, string? search)
    {
        // Arrange
        var staffs = ListStaffsBuilder.WithDefaultValues();
        int totalRecords = GetTotalRecords(staffs, roleType, search);

        SetUpHttpContext();

        var query = new ListStaffsQuery(roleType, pageIndex, pageSize, orderBy, isDescending, search);

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(staffs);
        _repositoryMock.Setup(r => r.CountAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(totalRecords);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.PagedInfo.TotalRecords.Should().Be(totalRecords);
        result.PagedInfo.TotalPages.Should().Be((int)Math.Ceiling((double)totalRecords / pageSize));

        if (!string.IsNullOrEmpty(search))
        {
            result.Value.Should().Contain(x => x.FirstName == search);
        }

        var firstStaff = result.Value.First();
        var lastStaff = result.Value.Last();

        if (orderBy == nameof(Staff.FullName))
        {
            firstStaff.FullName.Should().Be(staffs[0].FullName);
            lastStaff.FullName.Should().Be(staffs[^1].FullName);
        }
        else
        {
            firstStaff.StaffCode.Should().Be(staffs[0].StaffCode);
            lastStaff.StaffCode.Should().Be(staffs[^1].StaffCode);
        }

        _repositoryMock.Verify(r => r.ListAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.CountAsync(It.IsAny<StaffFilterSpec>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GivenQueryRequest_ShouldThrowNullOrEmptyException_WhenLocationClaimIsMissing()
    {
        // Arrange
        var query = new ListStaffsQuery(null, 1, 10, null, false, null);

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

        var query = new ListStaffsQuery(null, 1, 10, null, false, null);

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
