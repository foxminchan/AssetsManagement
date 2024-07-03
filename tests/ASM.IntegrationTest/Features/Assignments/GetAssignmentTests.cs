using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Assignments;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assignments;

public sealed class GetAssignmentTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenAssignmentId_ShouldReturnNotFound_IfAssignmentsNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/assignments/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenAssignmentId_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assignment = new AssignmentFaker().Generate(1);
        var assets = new AssetFaker().Generate(1);
        var staffs = new StaffFaker().Generate(1);
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };
        assets[0].CategoryId = category.Id;
        assignment[0].AssetId = assets[0].Id;
        assignment[0].StaffId = staffs[0].Id;
        assignment[0].CreatedBy = staffs[0].Id;
        assignment[0].UpdatedBy = staffs[0].Id;
        var id = assignment[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        await _factory.EnsureCreatedAndPopulateDataAsync(assignment);
        var response = await client.GetAsync($"/api/assignments/{id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var assignmentResponse = await response.Content.ReadFromJsonAsync<AssignmentDto>();
        assignmentResponse.Should().NotBeNull();
        assignmentResponse.Id.Should().Be(id);
    }
}
