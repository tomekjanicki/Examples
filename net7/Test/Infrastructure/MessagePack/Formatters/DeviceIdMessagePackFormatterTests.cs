using Code.Infrastructure.MessagePack;
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
        var result = MessagePackSerializerWrapper.Serialize(deviceId);
        var obj = MessagePackSerializerWrapper.Deserialize<DeviceId>(result);
        obj.Value.Should().Be(deviceId.Value);
    }

    [Fact]
    public void NullableShouldSerializeAndDeserialize()
    {
        var deviceId = (DeviceId?)null;
        var result = MessagePackSerializerWrapper.Serialize(deviceId);
        var obj = MessagePackSerializerWrapper.Deserialize<DeviceId?>(result);
        obj.Should().BeNull();
    }

    [Fact]
    public void ShouldNotDeserializeFromInvalidValue()
    {
        var result = MessagePackSerializerWrapper.Serialize(0);
        var func = () => MessagePackSerializerWrapper.Deserialize<DeviceId?>(result);
        func.Should().Throw<MessagePackSerializationException>();
    }
}