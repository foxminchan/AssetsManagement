using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.IdentityAggregate.Enums;
using ASM.Application.Features.Staffs.Create;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fixtures;
using Bogus;

namespace ASM.IntegrationTest.Features.Staffs;

public sealed class PostStaffTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync()
    {
        await _factory.StartContainersAsync();
        await _factory.EnsureCreatedAsync();
    }

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CreateStaffRequest(
            "Nhan",
            "Nguyen",
            new(1995, 10, 10),
            new(2021, 10, 10),
            Gender.Male,
            RoleType.Admin);

        // Act
        var response = await client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(CreateStaffRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/users", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

internal class InvalidData : IEnumerable<object[]>
{
    private readonly Faker _faker = new();

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new CreateStaffRequest(
                "",
                "Nguyen",
                new(2020, 10, 10),
                new(2021, 7, 31),
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new CreateStaffRequest(
                "Nhan",
                "",
                new(1995, 10, 10),
                new(2021, 7, 31),
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new CreateStaffRequest(
                "Nhan",
                "Nguyen",
                new(2020, 10, 10),
                new(2021, 7, 31),
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new CreateStaffRequest(
                "Nhan",
                "Nguyen",
                new(2002, 10, 10),
                new(2021, 7, 31),
                Gender.Male,
                RoleType.Admin)
        ];
        yield return
        [
            new CreateStaffRequest(
                _faker.Lorem.Sentence(1000),
                _faker.Lorem.Sentence(1000),
                new(2002, 10, 10),
                new(2021, 7, 30),
                Gender.Male,
                RoleType.Admin)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
