using System.Net;
using System.Net.Mime;
using ASM.IntegrationTest.Fixtures;

namespace ASM.IntegrationTest.Features.Reports;

public sealed class ExportAssetsByCategoryTests(ApplicationFactory<Program> factory)
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

        // Act
        var response = await client.GetAsync("/api/reports/assets-by-category/export");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task GivenEmptyOrderBy_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/reports/assets-by-category/export?orderBy=");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
