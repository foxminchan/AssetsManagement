using System.Text.Json.Serialization;

namespace ASM.Application.Domain.IdentityAggregate.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender : byte
{
    Male = 0,
    Female = 1
}
