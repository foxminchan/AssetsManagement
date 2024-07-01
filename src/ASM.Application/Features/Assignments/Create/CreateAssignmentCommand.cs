using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;

namespace ASM.Application.Features.Assignments.Create;

public sealed record CreateAssignmentCommand(
    Guid UserId,
    Guid AssetId,
    DateOnly AssignedDate,
    string Note) : ICommand<Result<Guid>>;

public sealed class CreateAssignmentHandler(IRepository<Assignment> repository)
    : ICommandHandler<CreateAssignmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
    {
        Assignment assignment = new(
            State.WaitingForAcceptance,
            request.AssignedDate,
            request.Note,
            request.AssetId,
            request.UserId);

        var result = await repository.AddAsync(assignment, cancellationToken);
        assignment.UpdateAssetState(request.AssetId);

        return result.Id;
    }
}
