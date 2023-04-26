using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Models;
using FluentAssertions;

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
}