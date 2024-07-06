using System.Text.Json.Serialization;

namespace ASM.Application.Domain.ReturningRequestAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum State : byte
{
    WaitingForReturning = 0,
    Completed = 1,
}
