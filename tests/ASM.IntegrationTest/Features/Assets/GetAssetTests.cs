using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.Shared;
using ASM.Application.Features.Assets;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Assets;

public sealed class GetAssetTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenAssetId_ShouldReturnNotFound_IfAssetsNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var id = Guid.NewGuid();

        // Act
        var response = await client.GetAsync($"/api/assets/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenAssetId_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var asset = new AssetFaker().Generate(1);
        asset[0].Location = Location.HoChiMinh;
        var category = new Category { Id = Guid.NewGuid(), Name = "Category 1", Prefix = "C1" };
        asset[0].CategoryId = category.Id;
        var id = asset[0].Id;

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        await _factory.EnsureCreatedAndPopulateDataAsync(asset);
        var response = await client.GetAsync($"/api/assets/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var assetResponse = await response.Content.ReadFromJsonAsync<AssetDto>();
        assetResponse.Should().NotBeNull();
        assetResponse.Id.Should().Be(id);
    }
}
