using Code.StronglyTypeIds;
using FluentAssertions;
using MessagePack;

namespace Test.Infrastructure.MessagePack.Formatters;

public class DeviceIdMessagePackFormatterTests
{
    [Fact]
    public void ShouldSerializeAndDeserializeWithValidObject()
    {
        var deviceId = (DeviceId)5;
        var result = MessagePackSerializer.Serialize(deviceId);
        var obj = MessagePackSerializer.Deserialize<DeviceId>(result);
        obj.Value.Should().Be(deviceId.Value);
    }

    [Fact]
    public void NullableShouldSerializeAndDeserialize()
    {
        var deviceId = (DeviceId?)null;
        var result = MessagePackSerializer.Serialize(deviceId);
        var obj = MessagePackSerializer.Deserialize<DeviceId?>(result);
        obj.Should().BeNull();
    }

    [Fact]
    public void ShouldNotDeserializeFromInvalidValue()
    {
        var result = MessagePackSerializer.Serialize(0);
        var func = () => MessagePackSerializer.Deserialize<DeviceId?>(result);
        func.Should().Throw<MessagePackSerializationException>();
    }
}