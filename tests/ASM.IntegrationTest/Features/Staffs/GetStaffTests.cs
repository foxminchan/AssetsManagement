using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Staffs;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class GetStaffTests(ApplicationFactory<Program> factory)
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
        var response = await client.GetAsync($"/api/users/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenStaffId_ShouldReturnNotFound_IfStaffsNotExistsInMyLocation()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staff = new StaffFaker().Generate(1);
        staff[0].Location = Location.HaNoi;
        var id = staff[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staff);
        var response = await client.GetAsync($"/api/users/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenStaffId_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staff = new StaffFaker().Generate(1);
        staff[0].Location = Location.HoChiMinh;
        staff[0].Users!.First().StaffId = staff[0].Id;
        var id = staff[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staff);
        var response = await client.GetAsync($"/api/users/{id}");
        var data = await response.Content.ReadFromJsonAsync<StaffDto>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data?.Id.Should().Be(id);
        data?.Location.Should().Be(Location.HoChiMinh);
    }
}
