using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Enums;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Staffs.Create;

public sealed record CreateStaffCommand(
    string FirstName,
    string LastName,
    DateOnly Dob,
    DateOnly JoinedDate,
    Gender Gender,
    RoleType RoleType) : ICommand<Result<Guid>>;

public sealed class CreateStaffHandler(
    IRepository<Staff> repository,
    IHttpContextAccessor httpContextAccessor) : ICommandHandler<CreateStaffCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
    {
        var semaphore = new SemaphoreSlim(1, 1);
        string staffCode;

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            var users = await repository.ListAsync(cancellationToken);
            staffCode = Staff.GenerateStaffCode(users);
        }
        finally
        {
            semaphore.Release();
        }

        var location = httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(x => x.Type == nameof(Location))
            ?.Value;

        Guard.Against.NullOrEmpty(location);

        Staff staff = new(
            request.FirstName,
            request.LastName,
            request.Dob,
            request.JoinedDate,
            request.Gender,
            staffCode,
            request.RoleType,
            Enum.Parse<Location>(location));

        staff.CreateStaffAccount(request.FirstName, request.LastName, request.RoleType, request.Dob, location, staff.Id);

        var result = await repository.AddAsync(staff, cancellationToken);

        return result.Id;
    }
}
