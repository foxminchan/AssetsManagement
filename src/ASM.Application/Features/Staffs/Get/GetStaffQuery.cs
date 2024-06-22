using Ardalis.GuardClauses;
using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;

namespace ASM.Application.Features.Staffs.Get;

public sealed record GetStaffQuery(Guid Id) : IQuery<Result<Staff>>;

public sealed class GetStaffHandler(IReadRepository<Staff> repository)
    : IQueryHandler<GetStaffQuery, Result<Staff>>
{
    public async Task<Result<Staff>> Handle(GetStaffQuery request, CancellationToken cancellationToken)
    {
        StaffFilterSpec spec = new(request.Id);

        var staff = await repository.FirstOrDefaultAsync(spec, cancellationToken);

        Guard.Against.NotFound(request.Id, staff);

        return staff;
    }
}
