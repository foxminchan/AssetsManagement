using System.Net;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class DeleteStaffTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenStaffId_ShouldReturnNotFound_IfStaffsNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.DeleteAsync($"/api/users/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenStaffId_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staff = new Staff()
        {
            FirstName = "Nhan",
            LastName = "Nguyen",
            Dob = new(2001, 08, 02),
            Gender = Gender.Male,
            StaffCode = "SD0208",
            Location = Location.HoChiMinh,
            RoleType = RoleType.Admin
        };
        var user = new ApplicationUser { UserName = "vinhdx", StaffId = staff.Id, };

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([staff]);
        await _factory.EnsureCreatedAndPopulateIdentityUserClaimsAsync(user);
        var response = await client.DeleteAsync($"/api/users/{staff.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
