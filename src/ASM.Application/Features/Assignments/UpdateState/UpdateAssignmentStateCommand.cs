using System.Security.Claims;
using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Constants;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Assignments.UpdateState;

public sealed record UpdateAssignmentStateCommand(Guid Id, State State) : ICommand<Result>;

public sealed class UpdateAssignmentStateHandler(
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    IRepository<Assignment> repository) : ICommandHandler<UpdateAssignmentStateCommand, Result>
{
    public async Task<Result> Handle(UpdateAssignmentStateCommand request, CancellationToken cancellationToken)
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

        assignment.UpdateState(request.State);

        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
