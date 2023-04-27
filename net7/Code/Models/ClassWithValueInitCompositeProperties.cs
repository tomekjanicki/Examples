using Code.StronglyTypeIds;
using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithValueInitCompositeProperties
{
    [Key(0)]
    public required DeviceId DeviceId { get; init; }

    [Key(1)]
    public DeviceId? OptionalDeviceId { get; init; }
}