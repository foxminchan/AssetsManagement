using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Microsoft.AspNetCore.Identity;

namespace ASM.Application.Features.Users.UpdatePassword;

public sealed record UpdatePasswordCommand(Guid Id, string OldPassword, string NewPassword) : ICommand<Result>;

public sealed class UpdatePasswordHandler(UserManager<ApplicationUser> userManager, IRepository<Staff> repository)
    : ICommandHandler<UpdatePasswordCommand, Result>
{
    public async Task<Result> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id.ToString());

        Guard.Against.NotFound(request.Id, user);

        if (user.AccountStatus == AccountStatus.FirstTime)
        {
            user.AccountStatus = AccountStatus.Active;
            var staffId = user.StaffId;
            Guard.Against.Null(staffId);
            var staff = await repository.GetByIdAsync((Guid)staffId, cancellationToken);
            staff?.UpdateActiveClaim(user);
        }

        await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        return Result.Success();
    }
}
