using System.Text.Json.Serialization;

namespace ASM.Application.Domain.AssetAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State : byte
{
    Available = 0,
    NotAvailable = 1,
    WaitingForRecycling = 3,
    Recycled = 4,
    Assigned = 5,
}
