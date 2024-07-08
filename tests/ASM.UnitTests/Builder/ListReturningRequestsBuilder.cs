using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.ReturningRequestAggregate;
using ASM.Application.Domain.ReturningRequestAggregate.Enums;

namespace ASM.UnitTests.Builder;

public static class ListReturningRequestsBuilder
{
    private static List<ReturningRequest> _returningRequests = [];

    public static List<ReturningRequest> WithDefaultValues()
    {
        var staffs = ListStaffsBuilder.WithDefaultValues();
        _returningRequests =
        [
            new(
                State.WaitingForReturning,
                null,
                staffs[0],
                null
            ),

            new(
                State.WaitingForReturning,
                null,
                staffs[1],
                null
            ),

            new(
                State.Completed,
                DateOnly.FromDateTime(DateTime.Now),
                staffs[3],
                staffs[1].Id
            )
        ];
        return _returningRequests;
    }
}
