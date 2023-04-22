using Code.StronglyTypeIds;
using MessagePack;

namespace Code.Models;

[MessagePackObject]
public sealed class ClassWithConstructorAndValueCompositeProperties
{
    [SerializationConstructor]
    public ClassWithConstructorAndValueCompositeProperties(DeviceId deviceId, DeviceId? optionalDeviceId)
    {
        DeviceId = deviceId;
        OptionalDeviceId = optionalDeviceId;
    }

    [Key(0)]
    public DeviceId DeviceId { get; }

    [Key(1)]
    public DeviceId? OptionalDeviceId { get; }
}