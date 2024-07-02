using Ardalis.Result;
using ASM.Application.Common.Interfaces;
using ASM.Application.Domain.AssignmentAggregate;
using ASM.Application.Domain.AssignmentAggregate.Enums;
using ASM.Application.Domain.AssignmentAggregate.Specifications;
using ASM.Application.Domain.IdentityAggregate;
using ASM.Application.Domain.IdentityAggregate.Specifications;

namespace ASM.Application.Features.Assignments.List;

public sealed record ListAssignmentsQuery(
    State? State,
    DateOnly? AssignedDate,
    int PageIndex,
    int PageSize,
    string? OrderBy,
    bool IsDescending,
    string? Search,
    Guid? AssetId) : IQuery<PagedResult<IEnumerable<Assignment>>>;

public sealed class ListAssignmentsHandler(
    IReadRepository<Assignment> assignmentRepository,
    IReadRepository<Staff> staffRepository)
    : IQueryHandler<ListAssignmentsQuery, PagedResult<IEnumerable<Assignment>>>
{
    public async Task<PagedResult<IEnumerable<Assignment>>> Handle(ListAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new AssignmentFilterSpec(
            request.State,
            request.AssignedDate,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.IsDescending,
            request.Search,
            request.AssetId);

        var assignments = await assignmentRepository.ListAsync(spec, cancellationToken);
        var staffIds = assignments.Select(a => a.StaffId).Concat(assignments.Select(a => a.UpdatedBy)).Distinct();
        var staffDictionary = (await staffRepository.ListAsync(new StaffFilterSpec(staffIds), cancellationToken))
            .ToDictionary(staff => staff.Id);

        foreach (var assignment in assignments)
        {
            staffDictionary.TryGetValue(assignment.StaffId, out var assignedTo);
            staffDictionary.TryGetValue(assignment.CreatedBy, out var assignedBy);
            assignment.AssignedTo = assignedTo?.UserName;
            assignment.AssignedBy = assignedBy?.UserName;
        }

        var totalRecords = await assignmentRepository.CountAsync(spec, cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.PageSize);

        PagedInfo pagedInfo = new(request.PageIndex, request.PageSize, totalPages, totalRecords);

        return new(pagedInfo, assignments);
    }
}
