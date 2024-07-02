using System.Security.Claims;
using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Assignments.GetOwn;

public sealed record GetOwnAssignmentQuery(Guid Id) : IQuery<Result<Assignment>>;

public sealed class GetOwnAssignmentHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    IReadRepository<Assignment> assignmentRepository,
    IReadRepository<Staff> staffRepository) : IQueryHandler<GetOwnAssignmentQuery, Result<Assignment>>
{
    public async Task<Result<Assignment>> Handle(GetOwnAssignmentQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guard.Against.NullOrEmpty(userId);

        var user = await userManager.FindByIdAsync(userId);
        var staffId = user?.StaffId;
        Guard.Against.Null(staffId);

        AssignmentFilterSpec spec = new(staffId.Value, request.Id);
        var assignment = await assignmentRepository.FirstOrDefaultAsync(spec, cancellationToken);
        Guard.Against.NotFound(request.Id, assignment);

        var assignedTo = await staffRepository.GetByIdAsync(assignment.StaffId, cancellationToken);
        var assignedBy = await staffRepository.GetByIdAsync(assignment.CreatedBy, cancellationToken);
        assignment.AssignedTo = assignedTo?.UserName;
        assignment.AssignedBy = assignedBy?.UserName;

        return assignment;
    }
}
