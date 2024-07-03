using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Features.Assets.List;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assets;

public sealed class ListAssetsTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly AssetFaker _faker = new();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidRequest_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = _faker.Generate(10);
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };
        
        foreach (var asset in assets)
        {
            asset.CategoryId = category.Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        var response = await client.GetAsync("/api/assets");

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
        var assets = _faker.Generate(5);
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };

        foreach (var asset in assets)
        {
            asset.CategoryId = category.Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        var response = await client.GetAsync($"/api/assets?pageIndex={pageIndex}&pageSize={pageSize}");
        var data = await response.Content.ReadFromJsonAsync<ListAssetResponse>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data!.Assets.Count.Should().Be(pageSize);
    }

    [Theory]
    [InlineData(1, -1, new[] { State.Available })]
    [InlineData(0, 1, new[] { State.Available })]
    [InlineData(1, 3, new[] { (State)100 })]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(int pageIndex, int pageSize, State[] states)
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = _faker.Generate(2);
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };

        foreach (var asset in assets)
        {
            asset.State = states[new Random().Next(0, states.Length)];
            asset.CategoryId = category.Id;
        }

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        var response =
            await client.GetAsync(
                $"/api/assets?pageIndex={pageIndex}&pageSize={pageSize}&state={
                    string.Join(",", states.Select(x => (int)x))}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
