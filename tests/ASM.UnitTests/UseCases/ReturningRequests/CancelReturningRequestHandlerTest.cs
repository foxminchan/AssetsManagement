using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Features.ReturningRequests.Cancel;
using Moq;
using MediatR;
using ASM.Application.Common.SeedWorks;

namespace ASM.UnitTests.UseCases.ReturningRequests;

public sealed class CancelReturningRequestHandlerTest
{
    private readonly Mock<IRepository<ReturningRequest>> _repositoryMock;
    private readonly CancelReturningRequestHandler _handler;
    private readonly Mock<IPublisher> _publisherMock;

    public CancelReturningRequestHandlerTest()
    {
        _publisherMock = new();
        _repositoryMock = new();
        _handler = new(_repositoryMock.Object, _publisherMock.Object);
    }

    [Fact]
    public async Task CancelReturningRequest_ShouldReturnSucess()
    {
        // Arrange 
        var request = new CancelReturningRequestCommand(Guid.NewGuid());
        var response = new ReturningRequest(Guid.NewGuid());

        _publisherMock.Setup(x => x.Publish(It.IsAny<EventBase>(), CancellationToken.None));
        _repositoryMock.Setup(x => x.GetByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(response);
        _repositoryMock.Setup(x => x.DeleteAsync(It.IsAny<ReturningRequest>(), CancellationToken.None));

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(x => x.GetByIdAsync(request.Id, CancellationToken.None), Times.Once);
    }    
}
