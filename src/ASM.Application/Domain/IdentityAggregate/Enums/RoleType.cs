using System.Text.Json.Serialization;

namespace ASM.Application.Domain.IdentityAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RoleType : byte
{
    Admin = 0,
    Staff = 1,
}
