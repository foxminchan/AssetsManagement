using ASM.Application.Domain.IdentityAggregate;

namespace ASM.UnitTests.Domain.Users;

public sealed class GenerateUserNameTest
{
    public const string FirstName = "Nhan";
    public const string LastName = "Nguyen Xuan";

    [Fact]
    public void GivenUsersAndFirstLastName_ShouldReturnExpectedUserName_WhenUserNameIsUnique()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new() { UserName = "khoitm" }
        };

        // Act
        var result = ApplicationUser.GenerateUserName(FirstName, LastName, users);

        // Assert
        result.Should().Be("nhannx");
    }

    [Fact]
    public void GivenUsersAndFirstLastName_ShouldAppendNumber_WhenUserNameIsNotUnique()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new() { UserName = "nhannx" }
        };

        // Act
        var result = ApplicationUser.GenerateUserName(FirstName, LastName, users);

        // Assert
        result.Should().Be("nhannx1");
    }

    [Fact]
    public void GivenUsersAndFirstLastName_ShouldAppendIncrementalNumber_WhenMultipleUserNamesAreNotUnique()
    {
        // Arrange
        var users = new List<ApplicationUser>
        {
            new() { UserName = "nhannx" },
            new() { UserName = "nhannx1" }
        };

        // Act
        var result = ApplicationUser.GenerateUserName(FirstName, LastName, users);

        // Assert
        result.Should().Be("nhannx2");
    }
}
