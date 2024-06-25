using ASM.Application.Domain.AssignmentAggregate;

namespace ASM.Application.Features.Assignments;

public static class EntityToDto
{
    public static AssignmentDto ToAssignmentDto(this Assignment assignment, int index = 1) =>
        new(index,
            assignment.Id,
            assignment.Asset?.AssetCode,
            assignment.Asset?.Name,
            assignment.Asset?.Specification,
            assignment.Asset?.Category?.Name,
            assignment.AssignedTo,
            assignment.AssignedBy,
            assignment.AssignedDate,
            assignment.State,
            assignment.Note);

    public static List<AssignmentDto> ToAssignmentDtos(this IEnumerable<Assignment> assignments, int total,
        bool isDescending) =>
        assignments.Select((assignment, index)
            => assignment.ToAssignmentDto(isDescending ? total - index : index + 1)).ToList();

    public static List<AssignmentDto> ToAssignmentDtos(this IEnumerable<Assignment> assignments) =>
        assignments.Select((assignment, index) => assignment.ToAssignmentDto(index + 1)).ToList();
}
