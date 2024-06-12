using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Staffs.Create;
using ASM.UnitTests.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ASM.UnitTests.UseCases.Staffs;

public sealed class CreateStaffHandlerTests
{
    private readonly Mock<IRepository<Staff>> _repositoryMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly CreateStaffHandler _handler;
    private readonly List<Staff> _staffs = ListStaffsBuilder.WithDefaultValues();

    public CreateStaffHandlerTests()
    {
        _repositoryMock = new();
        _httpContextAccessor = new();
        _handler = new(_repositoryMock.Object, _httpContextAccessor.Object);
    }

    [Fact]
    public async Task GivenCreateStaff_ShouldCreateStaff_WhenLocationValid()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateStaffCommand("Tran",
                                             "Khoi",
                                             new DateOnly(2003, 09, 05),
                                             new DateOnly(2024, 06, 20),
                                             Gender.Male,
                                             RoleType.Staff);
        var response = new Staff("Tran",
                                 "Khoi",
                                 new DateOnly(2003, 09, 05),
                                 new DateOnly(2024, 06, 20),
                                 Gender.Male,
                                 "SD0004",
                                 RoleType.Staff,
                                 Location.HoChiMinh);

        var claims = new List<Claim> { new(nameof(Location), nameof(Location.HoChiMinh)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        _repositoryMock.Setup(rm => rm.ListAsync(cancellationToken))
            .ReturnsAsync(_staffs);
        _repositoryMock.Setup(rm => rm.AddAsync(It.IsAny<Staff>(), cancellationToken)).ReturnsAsync(response);

        // Act
        var result = await _handler.Handle(request, cancellationToken);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(response.Id);
        _repositoryMock.Verify(r => r.ListAsync(cancellationToken), Times.Once());
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Staff>(), cancellationToken), Times.Once());
    }

    [Fact]
    public async Task GivenCreateStaff_ShouldCreateStaff_WhenLocationClaimIsMissing()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateStaffCommand("Tran",
                                             "Khoi",
                                             new DateOnly(2003, 09, 05),
                                             new DateOnly(2024, 06, 20),
                                             Gender.Male,
                                             RoleType.Staff);
        var response = new Staff("Tran",
                                 "Khoi",
                                 new DateOnly(2003, 09, 05),
                                 new DateOnly(2024, 06, 20),
                                 Gender.Male,
                                 "SD0004",
                                 RoleType.Staff,
                                 Location.HoChiMinh);

        _httpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());

        _repositoryMock.Setup(rm => rm.ListAsync(cancellationToken))
            .ReturnsAsync(_staffs);
        _repositoryMock.Setup(rm => rm.AddAsync(It.IsAny<Staff>(), cancellationToken)).ReturnsAsync(response);

        // Act
        Func<Task<Guid>> result = async () => await _handler.Handle(request, cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ArgumentNullException>();
        _repositoryMock.Verify(r => r.ListAsync(cancellationToken), Times.Once());
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Staff>(), cancellationToken), Times.Never());
    }

    [Fact]
    public async Task GivenCreateStaff_ShouldCreateStaff_WhenLocationClaimIsEmpty()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;
        var request = new CreateStaffCommand("Tran",
                                             "Khoi",
                                             new DateOnly(2003, 09, 05),
                                             new DateOnly(2024, 06, 20),
                                             Gender.Male,
                                             RoleType.Staff);
        var response = new Staff("Tran",
                                 "Khoi",
                                 new DateOnly(2003, 09, 05),
                                 new DateOnly(2024, 06, 20),
                                 Gender.Male,
                                 "SD0004",
                                 RoleType.Staff,
                                 Location.HoChiMinh);

        var claims = new List<Claim> { new(nameof(AccountStatus), nameof(AccountStatus.Active)) };
        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };
        _httpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        _repositoryMock.Setup(rm => rm.ListAsync(cancellationToken))
            .ReturnsAsync(_staffs);
        _repositoryMock.Setup(rm => rm.AddAsync(It.IsAny<Staff>(), cancellationToken)).ReturnsAsync(response);

        // Act
        Func<Task<Guid>> result = async () => await _handler.Handle(request, cancellationToken);

        // Assert
        await result.Should().ThrowAsync<ArgumentNullException>();
        _repositoryMock.Verify(r => r.ListAsync(cancellationToken), Times.Once());
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Staff>(), cancellationToken), Times.Never());
    }
}
