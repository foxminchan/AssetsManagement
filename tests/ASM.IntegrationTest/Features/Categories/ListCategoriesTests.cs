using System.Net;
using ASM.Application.Domain.AssetAggregate;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Categories;

public sealed class ListCategoriesTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var categories = new List<Category>
        {
            new("Category 1", "C1"), new("Category 2", "C2"), new("Category 3", "C3"), new("Category 4", "C4"),
        };

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(categories);
        var response = await client.GetAsync("/api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
