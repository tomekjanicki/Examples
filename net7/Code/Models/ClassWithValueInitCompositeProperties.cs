using Code.StronglyTypeIds;

namespace Code.Models;

public sealed class ClassWithValueInitCompositeProperties
{
    public required DeviceId DeviceId { get; init; }

    public DeviceId? OptionalDeviceId { get; init; }
}