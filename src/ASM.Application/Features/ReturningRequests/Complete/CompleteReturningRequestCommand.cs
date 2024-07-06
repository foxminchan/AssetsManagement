using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.ReturningRequestAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.ReturningRequests.Complete;

public sealed record CompleteReturningRequestCommand(Guid Id) : ICommand<Result>;

public sealed class CompleteReturningRequestHandler(
    IRepository<ReturningRequest> repository, 
    UserManager<ApplicationUser> userManager,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CompleteReturningRequestCommand, Result>
{
    public async Task<Result> Handle(CompleteReturningRequestCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.GetUserAsync(httpContextAccessor.HttpContext?.User!);

        Guard.Against.Null(user);

        var returningRequest = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, returningRequest);

        returningRequest.MarkComplete(user.StaffId);

        await repository.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
