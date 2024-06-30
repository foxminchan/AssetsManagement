using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Features.Categories.Create;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fixtures;
using Bogus;

namespace ASM.IntegrationTest.Features.Categories;

public sealed class PostCategoryTests(ApplicationFactory<Program> factory)
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
        var request = new CreateCategoryRequest("Category Name", "CN");

        // Act
        var response = await client.PostAsJsonAsync("/api/categories", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData("Category Name", "CN")]
    [InlineData("NashTech", "CN")]
    [InlineData("Category Name", "NT")]
    public async Task GivenValidRequest_ShouldReturnConflict_WhenHasExistsNameOrPrefix(string name, string prefix)
    {
        // Arrange
        var client = _factory.CreateClient();
        var category = new Category { Id = Guid.NewGuid(), Prefix = "CN", Name = "Category Name" };
        var request = new CreateCategoryRequest(name, prefix);

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        var response = await client.PostAsJsonAsync("/api/categories", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(CreateCategoryRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsJsonAsync("/api/categories", request);

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
            new CreateCategoryRequest(
                _faker.Random.String2(101),
                _faker.Random.String2(3))
        ];
        yield return
        [
            new CreateCategoryRequest(
                _faker.Random.String2(100),
                _faker.Random.String2(4))
        ];
        yield return
        [
            new CreateCategoryRequest(
                string.Empty,
                _faker.Random.String2(3))
        ];
        yield return
        [
            new CreateCategoryRequest(
                _faker.Random.String2(100),
                string.Empty)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
