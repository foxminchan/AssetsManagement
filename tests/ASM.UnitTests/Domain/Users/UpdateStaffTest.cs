using System.ComponentModel;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;

namespace ASM.UnitTests.Domain.Users;

public sealed class UpdateStaffTest
{
    private const string TestFirstName = "Dien";
    private const string TestLastName = "Truong Kim";
    private const string TestStaffCode = "SD0001";
    private readonly DateOnly TestDob = new(2002, 5, 11);
    private readonly DateOnly TestJoinedDate = new(2023, 10, 10);
    private readonly Gender TestGender = Gender.Male;
    private readonly RoleType TestRoleType = RoleType.Staff;

    [Fact]
    public void GivenValidData_ShouldReturnCorrectInformation()
    {
        // Arrange
        var staff = new Staff(
            TestFirstName,
            TestLastName,
            TestDob,
            TestJoinedDate,
            TestGender,
            TestStaffCode,
            TestRoleType,
            Location.HoChiMinh);
        var newDob = new DateOnly(2002, 11, 05);
        var newJoinedDate = new DateOnly(2020, 12, 11);
        var newGender = Gender.Female;
        var newRoleType = RoleType.Staff;

        // Act
        staff.Update(newDob, newJoinedDate, newGender, newRoleType);

        // Assert
        staff.Dob.Should().Be(newDob);
        staff.JoinedDate.Should().Be(newJoinedDate);
        staff.Gender.Should().Be(newGender);
        staff.RoleType.Should().Be(newRoleType);
    }

    [Fact]
    public void GivenInvalidGender_ShouldThrowInvalidEnumArgumentException()
    {
        // Arrange
        var staff = new Staff(
            TestFirstName,
            TestLastName,
            TestDob,
            TestJoinedDate,
            TestGender,
            TestStaffCode,
            TestRoleType,
            Location.HoChiMinh);

        // Act
        var act = () => staff.Update(TestDob, TestJoinedDate, (Gender)5, RoleType.Staff);

        // Assert
        act.Should().Throw<InvalidEnumArgumentException>();
    }

    [Fact]
    public void GivenInvalidRoleType_ShouldThrowInvalidEnumArgumentException()
    {
        // Arrange
        var staff = new Staff(TestFirstName, TestLastName, TestDob, TestJoinedDate, TestGender, TestStaffCode, TestRoleType, Location.HoChiMinh);

        // Act
        var act = () => staff.Update(TestDob, TestJoinedDate, Gender.Male, (RoleType)6);

        // Assert
        act.Should().Throw<InvalidEnumArgumentException>();
    }
}
