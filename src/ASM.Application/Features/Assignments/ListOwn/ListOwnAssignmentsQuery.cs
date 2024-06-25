using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Assignments.ListOwn;

public sealed record ListOwnAssignmentsQuery(string? OrderBy, bool IsDescending)
    : IQuery<IEnumerable<Assignment>>;

public sealed class ListOwnAssignmentsHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    IReadRepository<Assignment> assignmentRepository,
    IReadRepository<Staff> staffRepository) : IQueryHandler<ListOwnAssignmentsQuery, IEnumerable<Assignment>>
{
    public async Task<IEnumerable<Assignment>> Handle(ListOwnAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        Guard.Against.NullOrEmpty(userId);

        var user = await userManager.FindByIdAsync(userId);

        var staffId = user?.StaffId;

        Guard.Against.Null(staffId);

        AssignmentFilterSpec spec = new(staffId.Value, request.OrderBy, request.IsDescending);

        var assignments = await assignmentRepository.ListAsync(spec, cancellationToken);

        foreach (var assignment in assignments)
        {
            var assignedTo = await staffRepository.GetByIdAsync(assignment.StaffId, cancellationToken);
            var assignedBy = await staffRepository.GetByIdAsync(assignment.UpdatedBy, cancellationToken);
            assignment.AssignedTo = assignedTo?.UserName;
            assignment.AssignedBy = assignedBy?.UserName;
        }

        return assignments;
    }
}
