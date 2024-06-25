using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;

namespace ASM.UnitTests.Builder;

public static class ListAssignmentsBuilder
{
    private static List<Assignment> _assignments = [];

    public static List<Assignment> WithDefaultValues()
    {
        _assignments =
        [
            new()
            {
                State = State.WaitingForAcceptance,
                AssignedDate = DateOnly.FromDateTime(DateTime.Now),
                Note = "Note 1",
                StaffId = Guid.NewGuid(),
                UpdatedBy = Guid.NewGuid()
            },

            new()
            {
                State = State.Accepted,
                AssignedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                Note = "Note 2",
                StaffId = Guid.NewGuid(),
                UpdatedBy = Guid.NewGuid()
            },

            new()
            {
                State = State.IsRequested,
                AssignedDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2)),
                Note = "Note 3",
                StaffId = Guid.NewGuid(),
                UpdatedBy = Guid.NewGuid()
            }
        ];

        return _assignments;
    }
}
