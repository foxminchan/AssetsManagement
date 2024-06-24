using System.Text.Json.Serialization;

namespace ASM.Application.Domain.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Location : byte
{
    HoChiMinh = 0,
    HaNoi = 1,
    DaNang = 2,
}
