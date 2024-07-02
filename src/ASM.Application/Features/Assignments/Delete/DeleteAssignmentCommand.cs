using System.Security.Claims;
using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Assignments.Delete;

public sealed record DeleteAssignmentCommand(Guid Id) : ICommand<Result>;

public class DeleteAssignmentHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    IRepository<Assignment> repository) : ICommandHandler<DeleteAssignmentCommand, Result>
{
    public async Task<Result> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
    {
        Assignment? assignment;
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;

        if (claimsPrincipal?.FindFirst(x => x.Type == nameof(AuthRole))?.Value == AuthRole.Admin)
        {
            assignment = await repository.GetByIdAsync(request.Id, cancellationToken);
        }
        else
        {
            var userId = claimsPrincipal?.FindFirstValue(ClaimTypes.NameIdentifier);
            Guard.Against.NullOrEmpty(userId);

            var user = await userManager.FindByIdAsync(userId);
            var staffId = user?.StaffId;
            Guard.Against.Null(staffId);

            AssignmentFilterSpec spec = new(staffId.Value, request.Id);
            assignment = await repository.FirstOrDefaultAsync(spec, cancellationToken);
        }

        Guard.Against.NotFound(request.Id, assignment);

        assignment.Staff = null;
        assignment.Asset = null;

        await repository.DeleteAsync(assignment, cancellationToken);

        return Result.Success();
    }
}
