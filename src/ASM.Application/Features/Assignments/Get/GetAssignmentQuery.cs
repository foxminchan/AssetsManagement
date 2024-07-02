using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.IdentityAggregate;

namespace ASM.Application.Features.Assignments.Get;

public sealed record GetAssignmentQuery(Guid Id) : IQuery<Result<Assignment>>;

public sealed class GetAssignmentHandler(
    IReadRepository<Assignment> assignmentRepository,
    IReadRepository<Staff> staffRepository) : IQueryHandler<GetAssignmentQuery, Result<Assignment>>
{
    public async Task<Result<Assignment>> Handle(GetAssignmentQuery request, CancellationToken cancellationToken)
    {
        var assignment = await assignmentRepository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, assignment);

        var assignedTo = await staffRepository.GetByIdAsync(assignment.StaffId, cancellationToken);
        var assignedBy = await staffRepository.GetByIdAsync(assignment.CreatedBy, cancellationToken);
        assignment.AssignedTo = assignedTo?.UserName;
        assignment.AssignedBy = assignedBy?.UserName;

        return assignment;
    }
}
