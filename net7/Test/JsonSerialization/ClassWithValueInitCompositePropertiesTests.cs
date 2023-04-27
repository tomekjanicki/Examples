using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Models;
using Code.StronglyTypeIds;
using FluentAssertions;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithValueInitCompositePropertiesTests
{
    [Fact]
    public void DeserializeWithAllProperties()
    {
        const string jsonString = @"{""DeviceId"":1,""OptionalDeviceId"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithValueInitCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        result!.DeviceId.Value.Should().Be(1);
        result.OptionalDeviceId.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""DeviceId"":null,""OptionalDeviceId"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithValueInitCompositeProperties>(jsonString);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        const string jsonString = @"{""OptionalDeviceId"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithValueInitCompositeProperties>(jsonString);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithValueInitCompositeProperties { DeviceId = (DeviceId)5 };
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithValueInitCompositeProperties>(serialized);
        destObj.DeviceId.Value.Should().Be(5);
        destObj.OptionalDeviceId.Should().BeNull();
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeForceNull()
    {
        var sourceObj = new ClassWithValueInitCompositePropertiesTmp();
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var func = () => MessagePackSerializerWrapper.Deserialize<ClassWithValueInitCompositeProperties>(serialized);

        func.Should().Throw<MessagePackSerializationException>().WithInnerException<MessagePackSerializationException>();
    }

    [MessagePackObject]
    public sealed class ClassWithValueInitCompositePropertiesTmp
    {
        [Key(0)]
        public DeviceId? DeviceId { get; init; }

        [Key(1)]
        public DeviceId? OptionalDeviceId { get; init; }
    }
}