using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Assignments.Update;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assignments;

public sealed class UpdateAssignmentTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidAssignmentId_ShouldReturnNotFound_IfNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = new AssetFaker().Generate(1);
        var assignment = new AssignmentFaker().Generate(1);
        var request = new UpdateAssignmentRequest(
            assignment[0].Id,
            assets[0].Id,
            Guid.NewGuid(), 
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            assignment[0].Note!);

        // Act
        var response = await client.PutAsJsonAsync("/api/assignments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenValidAssignmentId_ShouldReturnSuccess()
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

        var request = new UpdateAssignmentRequest(
            assignment[0].Id,
            assets[0].Id,
            assignment[0].StaffId,
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            assignment[0].Note!);

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        await _factory.EnsureCreatedAndPopulateDataAsync(assignment);
        var response = await client.PutAsJsonAsync("/api/assignments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(InvalidUpdatedAssignmentData))]
    public async Task GivenInvalidData_ShouldReturnBadRequest(UpdateAssignmentRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("/api/assignments", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

public sealed class InvalidUpdatedAssignmentData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new UpdateAssignmentRequest(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now), 
                null!)
        ];
        yield return
        [
            new UpdateAssignmentRequest(
                Guid.Empty,
                Guid.Empty,
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                "Note")
        ];
        yield return
        [
            new UpdateAssignmentRequest(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.Empty,
                DateOnly.FromDateTime(DateTime.Now),
                "Note")
        ];
        yield return
        [
            new UpdateAssignmentRequest(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateOnly.FromDateTime(DateTime.Now),
                string.Empty)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
