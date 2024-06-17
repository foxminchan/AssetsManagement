using ASM.Application.Domain.IdentityAggregate;

namespace ASM.UnitTests.Domain.Users;

public sealed class GenerateStaffCodeTest
{
    [Fact]
    public void GivenListUsers_ShouldReturnSD0001_WhenNoExistingStaffCode()
    {
        // Arrange
        var users = new List<Staff>();

        // Act
        var result = Staff.GenerateStaffCode(users);

        // Assert
        result.Should().Be("SD0001");
    }

    [Fact]
    public void GivenListUsers_ShouldReturnNextStaffCode_WhenExistingStaffCodesPresent()
    {
        // Arrange
        var users = new List<Staff>
        {
            new() { StaffCode = "SD0001" },
            new() { StaffCode = "SD0002" }
        };

        // Act
        var result = Staff.GenerateStaffCode(users);

        // Assert
        result.Should().Be("SD0003");
    }

    [Fact]
    public void GivenListUsers_ShouldHandleGapsInStaffCodes()
    {
        // Arrange
        var users = new List<Staff>
        {
            new() { StaffCode = "SD0001" },
            new() { StaffCode = "SD0003" }
        };

        // Act`
        var result = Staff.GenerateStaffCode(users);

        // Assert
        result.Should().Be("SD0002");
    }

    [Fact]
    public void GivenListUsers_ShouldHandleLargeNumberOfStaffCodes()
    {
        // Arrange
        var users = new List<Staff>();
        for (int i = 1; i <= 999; i++)
        {
            users.Add(new() { StaffCode = $"SD{i:D4}" });
        }

        // Act
        var result = Staff.GenerateStaffCode(users);

        // Assert
        result.Should().Be("SD1000");
    }
}
