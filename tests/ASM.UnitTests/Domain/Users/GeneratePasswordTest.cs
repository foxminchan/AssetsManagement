using ASM.Application.Domain.IdentityAggregate;

namespace ASM.UnitTests.Domain.Users;

public sealed class GeneratePasswordTest
{
    private const string UserName = "nhannx";

    [Fact]
    public void GivenUserNameAndDob_ShouldReturnCorrectFormat()
    {
        // Arrange
        var dob = new DateOnly(1990, 5, 23);

        // Act
        var result = ApplicationUser.GeneratePassword(UserName, dob);

        // Assert
        result.Should().Be("nhannx@23051990");
    }
}
