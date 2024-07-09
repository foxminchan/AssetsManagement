using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Features.Assets.Update;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fakers;
using ASM.IntegrationTest.Fixtures;
using Bogus;

namespace ASM.IntegrationTest.Features.Assets;

public sealed class UpdateAssetTests(ApplicationFactory<Program> factory)
    : IClassFixture<ApplicationFactory<Program>>, IAsyncLifetime
{
    private readonly ApplicationFactory<Program> _factory = factory.WithDbContainer();

    public async Task InitializeAsync() => await _factory.StartContainersAsync();

    public async Task DisposeAsync() => await _factory.StopContainersAsync();

    [Fact]
    public async Task GivenValidAssetId_ShouldReturnNotFound_IfNotExists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = new AssetFaker().Generate(1);
        assets[0].Category = new("Category Name", "CN");
        var request = new UpdateAssetRequest(
            Guid.NewGuid(),
            "Asset Name",
            "Asset Specification",
            DateOnly.FromDateTime(DateTime.Now), 
            State.WaitingForRecycling);

        // Act
        var response = await client.PutAsJsonAsync("/api/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GivenValidAssetId_ShouldReturnSuccess()
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = new AssetFaker().Generate(1);
        assets[0].Category = new("Category Name", "CN");
        var request = new UpdateAssetRequest(
            assets[0].Id,
            "Asset Name",
            "Asset Specification",
            DateOnly.FromDateTime(DateTime.Now), 
            State.WaitingForRecycling);

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        var response = await client.PutAsJsonAsync("/api/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(InvalidUpdateAssetData))]
    public async Task GivenInvalidAssetId_ShouldReturnBadRequest(UpdateAssetRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();
        var assets = new AssetFaker().Generate(1);
        assets[0].Category = new("Category Name", "CN");
        request = request with { Id = assets[0].Id };

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync(assets);
        var response = await client.PutAsJsonAsync("/api/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

internal class InvalidUpdateAssetData : IEnumerable<object[]>
{
    private readonly Faker _faker = new();

    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new UpdateAssetRequest(
                Guid.Empty,
                _faker.Lorem.Sentence(1000),
                _faker.Commerce.ProductDescription(),
                DateOnly.FromDateTime(DateTime.Now),
                State.WaitingForRecycling)
        ];
        yield return
        [
            new UpdateAssetRequest(
                Guid.Empty,
                string.Empty,
                _faker.Commerce.ProductDescription(),
                DateOnly.FromDateTime(DateTime.Now),
                State.WaitingForRecycling)
        ];
        yield return
        [
            new UpdateAssetRequest(
                Guid.Empty,
                string.Empty,
                _faker.Commerce.ProductDescription(),
                DateOnly.FromDateTime(DateTime.Now),
                State.Assigned)
        ];
        yield return
        [
            new UpdateAssetRequest(
                Guid.Empty,
                string.Empty,
                _faker.Commerce.ProductDescription(),
                DateOnly.FromDateTime(DateTime.Now),
                (State)99)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
