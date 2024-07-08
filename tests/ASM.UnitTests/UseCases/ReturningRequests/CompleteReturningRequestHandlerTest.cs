using System.Security.Claims;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Features.ReturningRequests.Complete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Location = ASM.Application.Domain.Shared.Location;

namespace ASM.UnitTests.UseCases.ReturningRequests;

public sealed class CompleteReturningRequestHandlerTest
{
    private readonly Mock<IRepository<ReturningRequest>> _repositoryMock;
    private readonly CompleteReturningRequestHandler _handler;
    private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

    public CompleteReturningRequestHandlerTest()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        _httpContextAccessorMock = new();
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object, _userManagerMock.Object, _httpContextAccessorMock.Object);
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
    public async Task CompleteReturningRequest_ShouldReturnSucess()
    {
        // Arrange 
        var request = new CompleteReturningRequestCommand(Guid.NewGuid());
        var response = new ReturningRequest(Guid.NewGuid());
        var user = new ApplicationUser
        {
            StaffId = Guid.NewGuid(),
        };

        _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
        SetUpHttpContext();
        _repositoryMock.Setup(x => x.GetByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(response);
        _repositoryMock.Setup(x => x.SaveChangesAsync(CancellationToken.None));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(x => x.GetByIdAsync(request.Id, CancellationToken.None), Times.Once);
    }

    [Fact]
    public void  GivenNullAcceptBy_ShouldThrowNullException()
    {
        // Arrange 
        var response = new ReturningRequest(Guid.NewGuid());

        var act = () => response.MarkComplete(null);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}
