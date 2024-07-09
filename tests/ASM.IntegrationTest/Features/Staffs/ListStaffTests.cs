using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Staffs.List;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class ListStaffTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly StaffFaker _faker = new();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var staffs = _faker.Generate(10);

        foreach (var staff in staffs)
        {
            staff.Users!.First().StaffId = staff.Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        var response = await client.GetAsync("/api/users");
        var data = await response.Content.ReadFromJsonAsync<ListStaffResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.Users.Should().NotBeNull();
    }

    [Theory]
    [InlineData(1, 1)]
    public async Task GivenValidRequest_ShouldReturnSuccess_WithFilter(int pageIndex, int pageSize)
    {
        // Arrange
        var client = _factory.CreateClient();
        var staffs = _faker.Generate(10);

        foreach (var staff in staffs)
        {
            staff.Users!.First().StaffId = staff.Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(staffs);
        var response =
            await client.GetAsync(
                $"/api/users?pageIndex={pageIndex}&pageSize={pageSize}");
        var data = await response.Content.ReadFromJsonAsync<ListStaffResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.Users.Count.Should().Be(pageSize);
    }

    [Theory]
    [InlineData(1, -1, nameof(Staff.StaffCode), false, RoleType.Admin)]
    [InlineData(0, 1, nameof(Staff.StaffCode), false, RoleType.Admin)]
    [InlineData(0, 1, nameof(Staff.StaffCode), false, (RoleType)9)]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(int pageIndex, int pageSize, string orderBy,
        bool isDescending, RoleType roleType)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response =
            await client.GetAsync(
                $"/api/users?pageIndex={pageIndex}&pageSize={pageSize}&orderBy={orderBy}&isDescending={isDescending}&roleType={roleType}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
