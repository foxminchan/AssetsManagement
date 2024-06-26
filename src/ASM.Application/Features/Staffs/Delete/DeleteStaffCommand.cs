﻿using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;

namespace ASM.Application.Features.Staffs.Delete;

public sealed record DeleteStaffCommand(Guid Id) : ICommand<Result>;

public sealed class DeleteStaffHandler(IRepository<Staff> repository)
    : ICommandHandler<DeleteStaffCommand, Result>
{
    public async Task<Result> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, staff);
        staff.Delete();
        if (staff.Users is not null)
        {
            var user = staff.Users.First();
            staff.UpdateClaim(user);
        }

        await repository.UpdateAsync(staff, cancellationToken);

        return Result.Success();
    }
}