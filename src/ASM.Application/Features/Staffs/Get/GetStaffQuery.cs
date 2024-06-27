using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;
using ASM.Application.Domain.Shared;
using Microsoft.AspNetCore.Http;

namespace ASM.Application.Features.Staffs.Get;

public sealed record GetStaffQuery(Guid Id) : IQuery<Result<Staff>>;

public sealed class GetStaffHandler(IReadRepository<Staff> repository, IHttpContextAccessor httpContextAccessor)
    : IQueryHandler<GetStaffQuery, Result<Staff>>
{
    public async Task<Result<Staff>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        var location = httpContextAccessor.HttpContext?.User.Claims
            .First(x => x.Type == nameof(Location)).Value;

        Guard.Against.NullOrEmpty(location);

        StaffFilterSpec spec = new(request.Id, Enum.Parse<Location>(location));

        var staff = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        Guard.Against.NotFound(request.Id, staff);

        return staff;
    }
}
