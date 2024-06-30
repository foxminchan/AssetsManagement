using System.Net;
using ASM.Application.Domain.IdentityAggregate;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class DeleteStaffTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly StaffFaker _faker = new();

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
        var staff = _faker.Generate(1);
        var user = new ApplicationUser { UserName = "vinhdx", StaffId = staff[0].Id, };

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staff);
        await _factory.EnsureCreatedAndPopulateIdentityUserClaimsAsync(user);
        var response = await client.DeleteAsync($"/api/users/{staff[0].Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
