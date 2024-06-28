using System.Security.Claims;
using Ardalis.GuardClauses;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;
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

        var staffIds = assignments.Select(a => a.StaffId).Concat(assignments.Select(a => a.UpdatedBy)).Distinct();
        var staffDictionary = (await staffRepository.ListAsync(new StaffFilterSpec(staffIds), cancellationToken))
            .ToDictionary(staff => staff.Id);

        foreach (var assignment in assignments)
        {
            staffDictionary.TryGetValue(assignment.StaffId, out var assignedTo);
            staffDictionary.TryGetValue(assignment.UpdatedBy, out var assignedBy);
            assignment.AssignedTo = assignedTo?.UserName;
            assignment.AssignedBy = assignedBy?.UserName;
        }

        return assignments;
    }
}
