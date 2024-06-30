using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Staffs.Update;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class UpdateStaffTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly StaffFaker _staffFaker = new();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidStaffId_ShouldReturnNotFound_IfNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staff = _staffFaker.Generate(1);
        var id = staff[0].Id;

        // Act
        var response = await client.PutAsJsonAsync("/api/users", new UpdateStaffRequest(
            id,
            staff[0].Dob,
           new(2024, 7, 4),
            staff[0].Gender,
            staff[0].RoleType));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenValidStaffId_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staff = _staffFaker.Generate(1);
        var id = staff[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staff);
        var response = await client.PutAsJsonAsync("/api/users", new UpdateStaffRequest(
            id,
            new(2002, 10, 10),
            new(2021, 7, 4),
            staff[0].Gender,
            staff[0].RoleType));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(InvalidUpdatedData))]
    public async Task GivenInvalidData_ShouldReturnBadRequest(UpdateStaffRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PutAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

internal sealed class InvalidUpdatedData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new UpdateStaffRequest(
                Guid.Empty,
                DateOnly.MinValue,
                DateOnly.MinValue,
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new UpdateStaffRequest(
                Guid.Empty,
                new(2002, 10, 10),
                new(2021, 7, 30),
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new UpdateStaffRequest(
                Guid.Empty,
                new(2002, 10, 10),
                new(2021, 7, 30),
                (Gender)5,
                (RoleType)10)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
