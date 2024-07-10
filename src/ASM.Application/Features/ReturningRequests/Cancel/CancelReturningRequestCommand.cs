using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.ReturningRequestAggregate;
using MediatR;

namespace ASM.Application.Features.ReturningRequests.Cancel;

public sealed record CancelReturningRequestCommand(Guid Id) : ICommand<Result>;

public sealed class CancelReturningRequestHandler(IRepository<ReturningRequest> repository,
    IPublisher publisher)
    : ICommandHandler<CancelReturningRequestCommand, Result>
{
    public async Task<Result> Handle(CancelReturningRequestCommand request, CancellationToken cancellationToken)
    {
        var returningRequest = await repository.GetByIdAsync(request.Id, cancellationToken);
        Guard.Against.NotFound(request.Id, returningRequest);

        returningRequest.CancelledReturnedAssignment();
        await publisher.Publish(returningRequest.DomainEvents.First(), cancellationToken);

        await repository.DeleteAsync(returningRequest, cancellationToken);

        return Result.Success();
    }
}
