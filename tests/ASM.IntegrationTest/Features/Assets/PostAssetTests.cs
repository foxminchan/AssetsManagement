using System.Collections;
using System.Net;
using System.Net.Http.Json;
using ASM.Application.Domain.AssetAggregate;
using ASM.Application.Domain.AssetAggregate.Enums;
using ASM.Application.Features.Assets.Create;
using ASM.IntegrationTest.Extensions;
using ASM.IntegrationTest.Fixtures;
using Bogus;

namespace ASM.IntegrationTest.Features.Assets;

public sealed class PostAssetTests(ApplicationFactory<Program> factory)
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
        var category = new Category { Id = Guid.NewGuid(), Prefix = "CN", Name = "Category Name" };
        var request = new CreateAssetRequest(
            "Asset Name",
            "Asset Specification",
            new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day),
            State.WaitingForRecycling,
            category.Id);

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        var response = await client.PostAsJsonAsync("/api/assets", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [ClassData(typeof(InvalidData))]
    public async Task GivenInvalidRequest_ShouldReturnBadRequest(CreateAssetRequest request)
    {
        // Arrange
        var client = _factory.CreateClient();
        var category = new Category { Id = Guid.NewGuid(), Prefix = "CN", Name = "Category Name" };
        request = request with { CategoryId = category.Id };

        // Act
        await _factory.EnsureCreatedAndPopulateDataAsync([category]);
        var response = await client.PostAsJsonAsync("/api/assets", request);

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
            new CreateAssetRequest(
                _faker.Lorem.Sentence(1000),
                _faker.Commerce.ProductDescription(),
                _faker.Date.FutureDateOnly(),
                State.WaitingForRecycling,
                Guid.Empty)
        ];
        yield return
        [
            new CreateAssetRequest(
                _faker.Commerce.ProductName(),
                _faker.Commerce.ProductDescription(),
                _faker.Date.PastDateOnly(),
                State.WaitingForRecycling,
                Guid.Empty)
        ];
        yield return
        [
            new CreateAssetRequest(
                string.Empty,
                _faker.Commerce.ProductDescription(),
                _faker.Date.FutureDateOnly(),
                State.WaitingForRecycling,
                Guid.Empty)
        ];
        yield return
        [
            new CreateAssetRequest(
                _faker.Commerce.ProductName(),
                _faker.Commerce.ProductDescription(),
                _faker.Date.FutureDateOnly(),
                (State)90,
                Guid.Empty)
        ];
        yield return
        [
            new CreateAssetRequest(
                _faker.Commerce.ProductName(),
                _faker.Lorem.Sentence(3000),
                _faker.Date.FutureDateOnly(),
                (State)90,
                Guid.Empty)
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
