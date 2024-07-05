using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Infrastructure.Persistence;

namespace ASM.Application.Features.Assignments.Update;

public sealed record UpdateAssignmentCommand(
    Guid Id,
    Guid AssetId,
    Guid UserId,
    DateOnly AssignedDate,
    string Note) : ICommand<Result>;

public sealed class UpdateAssignmentHandler(IRepository<Assignment> repository) 
    : ICommandHandler<UpdateAssignmentCommand, Result>
{
    public async Task<Result> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
    {
        var assignment = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, assignment);
       
        if (assignment.AssetId != request.AssetId)
        {
            assignment.UpdateAssetState(request.AssetId, assignment.AssetId);
        }

        assignment.Update(request.UserId, request.AssetId, request.AssignedDate, request.Note);
        await repository.UpdateAsync(assignment, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return Result.Success();

    }
}
