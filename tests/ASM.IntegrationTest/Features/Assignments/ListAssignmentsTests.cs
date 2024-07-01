using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Features.Assignments.List;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assignments;

public sealed class ListAssignmentsTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly AssignmentFaker _faker = new();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assignments = _faker.Generate(10);
        var assets = new AssetFaker().Generate(1);
        var staffs = new StaffFaker().Generate(1);

        foreach (var assignment in assignments)
        {
            assignment.AssetId = assets[0].Id;
            assignment.StaffId = staffs[0].Id;
            assignment.CreatedBy = staffs[0].Id;
            assignment.UpdatedBy = staffs[0].Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        await _factory.EnsureCreatedAndPopulateDataAsync(assignments);
        var response = await client.GetAsync("/api/assignments");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 3)]
    public async Task GivenValidRequest_ShouldReturnSuccess_WithFilter(int pageIndex, int pageSize)
    {
        // Arrange
        var client = _factory.CreateClient();
        var assignments = _faker.Generate(10);
        var assets = new AssetFaker().Generate(1);
        var staffs = new StaffFaker().Generate(1);

        foreach (var assignment in assignments)
        {
            assignment.AssetId = assets[0].Id;
            assignment.StaffId = staffs[0].Id;
            assignment.CreatedBy = staffs[0].Id;
            assignment.UpdatedBy = staffs[0].Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        await _factory.EnsureCreatedAndPopulateDataAsync(assignments);
        var response =
            await client.GetAsync(
                $"/api/assignments?pageIndex={pageIndex}&pageSize={pageSize}");
        var data = await response.Content.ReadFromJsonAsync<ListAssignmentsResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.Assignments.Count.Should().Be(pageSize);
    }

    [Theory]
    [InlineData(1, -1, nameof(Assignment.Asset.AssetCode), false, State.Assigned)]
    [InlineData(0, 1, nameof(Assignment.Asset.AssetCode), false, State.Assigned)]
    [InlineData(0, 1, nameof(Assignment.Asset.AssetCode), false, (State)9)]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(int pageIndex, int pageSize, string orderBy,
        bool isDescending, State state)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                $"/api/assignments?pageIndex={pageIndex}&pageSize={pageSize}&orderBy={orderBy}&isDescending={isDescending}&state={state}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
