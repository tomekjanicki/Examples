using System.Text.Json;
using Code.Infrastructure.Json;
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
        var result = SerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        result!.DeviceId.Value.Should().Be(1);
        result.OptionalDeviceId.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""DeviceId"":null,""OptionalDeviceId"":null}";
        var func = () => SerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        //not good
        const string jsonString = @"{""OptionalDeviceId"":null}";
        var result = SerializerWrapper.Deserialize<ClassWithConstructorAndValueCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        var func = () => result!.DeviceId.Value;
        func.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndValueCompositeProperties((DeviceId)5, null);
        var serialized = MessagePackSerializer.Serialize(sourceObj);
        var destObj = MessagePackSerializer.Deserialize<ClassWithConstructorAndValueCompositeProperties>(serialized);
        destObj.DeviceId.Value.Should().Be(5);
        destObj.OptionalDeviceId.Should().BeNull();
    }

}