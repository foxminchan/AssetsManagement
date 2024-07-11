using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;
using FluentValidation;

namespace ASM.Application.Features.ReturningRequests.Cancel;

public sealed class CancelReturningRequestValidator : AbstractValidator<CancelReturningRequestCommand>
{
    public CancelReturningRequestValidator(StateValidator stateValidator)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x).SetValidator(stateValidator);
    }
}

public sealed class StateValidator : AbstractValidator<CancelReturningRequestCommand>
{
    private readonly IReadRepository<ReturningRequest> _repository;

    public StateValidator(IReadRepository<ReturningRequest> repository)
    {
        _repository = repository;

        RuleFor(x => x)
            .MustAsync(CorrectState)
            .WithMessage("Invalid returning request");
    }

    private async Task<bool> CorrectState(CancelReturningRequestCommand command, CancellationToken cancellationToken)
    {
        var returnRequest = await _repository.GetByIdAsync(command.Id, cancellationToken);
        Guard.Against.NotFound(command.Id, returnRequest);
        return returnRequest.State != State.Completed;
    }
}
