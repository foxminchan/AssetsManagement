using System.Net;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Domain.Shared;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assets;

public sealed class DeleteAssetTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    private readonly AssetFaker _faker = new();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenAssetId_ShouldReturnNotFound_AssetNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assetId = Guid.NewGuid();

        // Act
        var response = await client.DeleteAsync($"/api/assets/{assetId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenAssetId_ShouldReturnNoContent()
    {
        // Arrange
        var client = _factory.CreateClient();
        var asset = _faker.Generate(1);
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };
        asset[0].State = State.Available;
        asset[0].Location = Location.HoChiMinh;
        asset[0].CategoryId = category.Id;
        var id = asset[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(asset);
        var response = await client.DeleteAsync($"/api/assets/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
