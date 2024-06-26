using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;
using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace ASM.Application.Features.Staffs.Update;

public sealed record UpdateStaffCommand(
        Guid Id,
        DateOnly Dob,
        DateOnly JoinedDate,
        Gender Gender,
        RoleType RoleType) : ICommand<Result>;

public sealed class UpdateStaffHandler(IRepository<Staff> repository) : ICommandHandler<UpdateStaffCommand, Result>
{
    public async Task<Result> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, user);

        user.Update(request.Dob, request.JoinedDate, request.Gender, request.RoleType);

        await repository.UpdateAsync(user, cancellationToken);

        return Result.Success();
    }
}