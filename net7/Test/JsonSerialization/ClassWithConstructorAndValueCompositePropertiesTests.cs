using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Models;
using Code.StronglyTypeIds;
using FluentAssertions;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithConstructorAndValueCompositePropertiesTests
{
    [Fact]
    public void DeserializeWithAllProperties()
    {
        const string jsonString = @"{""DeviceId"":1,""OptionalDeviceId"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        result!.DeviceId.Value.Should().Be(1);
        result.OptionalDeviceId.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""DeviceId"":null,""OptionalDeviceId"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        //not good
        const string jsonString = @"{""OptionalDeviceId"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        var func = () => result!.DeviceId.Value;
        func.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndValueCompositeProperties((DeviceId)5, null);
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(serialized);
        destObj.DeviceId.Value.Should().Be(5);
        destObj.OptionalDeviceId.Should().BeNull();
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeForceNull()
    {
        var sourceObj = new ClassWithConstructorAndValueCompositePropertiesTmp(null, null);
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var func = () => MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(serialized);

        func.Should().Throw<MessagePackSerializationException>().WithInnerException<MessagePackSerializationException>();
    }

    [MessagePackObject]
    public sealed class ClassWithConstructorAndValueCompositePropertiesTmp
    {
        [SerializationConstructor]
        public ClassWithConstructorAndValueCompositePropertiesTmp(DeviceId? deviceId, DeviceId? optionalDeviceId)
        {
            DeviceId = deviceId;
            OptionalDeviceId = optionalDeviceId;
        }

        [Key(0)]
        public DeviceId? DeviceId { get; }

        [Key(1)]
        public DeviceId? OptionalDeviceId { get; }
    }
}