using System.Text.Json.Serialization;

namespace ASM.Application.Domain.AssignmentAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State : byte
{
    WaitingForAcceptance = 0,
    Accepted = 1,
    IsRequested = 2
}
