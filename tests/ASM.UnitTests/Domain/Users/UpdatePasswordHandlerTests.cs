using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Users.UpdatePassword;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ASM.UnitTests.UseCases.Users;

public sealed class UpdatePasswordHandlerTests
{
    private readonly UpdatePasswordHandler handler;
    private readonly Mock<UserManager<ApplicationUser>> userManager;

    public UpdatePasswordHandlerTests()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        handler = new(userManager.Object);
    }

    private static string TestPassword = "TestPassword";
    private static string NewTestPassword = "NewTestPassword";

    private static ApplicationUser SetupUserManagerWithTestUser(Mock<UserManager<ApplicationUser>> userManager, AccountStatus accountStatus)
    {
        ApplicationUser testUser = new() { UserName = "test", AccountStatus = accountStatus };
        userManager.Setup(um => um.CreateAsync(testUser, TestPassword));
        userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(testUser);
        userManager.Setup(um => um.GetClaimsAsync(testUser))
            .ReturnsAsync([new("Status", accountStatus.ToString())]);
        return testUser;
    }

    [Fact]
    public async Task GivenValidRequest_ShouldUpdatePassword_IfUserExists()
    {
        // Arrange
        SetupUserManagerWithTestUser(userManager, AccountStatus.FirstTime);

        var command = new UpdatePasswordCommand(Guid.NewGuid(), TestPassword, NewTestPassword);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(Result.Success());
        userManager.Verify(um =>
            um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async Task GivenValidRequest_ShouldThrowNotFoundException_IfUserNotExist()
    {
        // Arrange
        userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser?)null);

        var command = new UpdatePasswordCommand(Guid.NewGuid(), TestPassword, NewTestPassword);

        // Act
        Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
        userManager.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once());
        userManager.Verify(um =>
            um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task GivenValidRequest_ShouldUpdateAccountStatusToActive_IfFirstTime()
    {
        // Arrange
        ApplicationUser testUser = SetupUserManagerWithTestUser(userManager, AccountStatus.FirstTime);

        var command = new UpdatePasswordCommand(Guid.NewGuid(), TestPassword, NewTestPassword);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        testUser.AccountStatus.Should().Be(AccountStatus.Active);
    }
}
