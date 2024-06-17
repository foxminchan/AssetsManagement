using System.Text.Json.Serialization;

namespace ASM.Application.Domain.IdentityAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AccountStatus : byte
{
    FirstTime = 0,
    Active = 1,
    Deactivated = 2
}
