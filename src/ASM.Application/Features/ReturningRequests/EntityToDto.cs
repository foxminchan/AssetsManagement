using ASM.Application.Domain.ReturningRequestAggregate;

namespace ASM.Application.Features.ReturningRequests;

public static class EntityToDto
{
    public static ReturningRequestDto ToReturningRequestDto(this ReturningRequest returningRequest, int index = 1) =>
        new(index,
            returningRequest.Id,
            returningRequest.Assignment?.Asset?.AssetCode,
            returningRequest.Assignment?.Asset?.Name,
            returningRequest.
            RequestedBy,
            returningRequest.Assignment?.AssignedDate,
            returningRequest.AcceptedBy,
            returningRequest.ReturnedDate,
            returningRequest.State);

    public static List<ReturningRequestDto> ToReturningRequestDtos(this IEnumerable<ReturningRequest> returningRequests,
        int pageNumber, int pageSize, int total, bool isDescending) =>
        returningRequests.Select((returningRequest, index)
            => returningRequest.ToReturningRequestDto(isDescending
                ? (total - index - (pageNumber - 1) * pageSize)
                : (index + 1 + (pageNumber - 1) * pageSize))).ToList();
}
