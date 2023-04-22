using System.Text.Json;
using Code.Infrastructure.Json;
using Code.StronglyTypeIds;
using FluentAssertions;

namespace Test.Infrastructure.Json.Converters;

public class DeviceIdJsonConverterTests
{
    [Fact]
    public void ShouldSerializeToJson()
    {
        var deviceId = (DeviceId)5;
        var result = SerializerWrapper.Serialize(deviceId);
        result.Should().Be(@"5");
    }

    [Fact]
    public void ShouldDeserializeFromValidJsonString()
    {
        const string jsonString = @"5";
        var result = SerializerWrapper.Deserialize<DeviceId>(jsonString);
        result.Should().NotBeNull();
        result.Value.Should().Be(5);
    }

    [Fact]
    public void ShouldNotDeserializeFromInvalidValueJsonString()
    {
        const string jsonString = @"0";
        var result = () => SerializerWrapper.Deserialize<DeviceId>(jsonString);
        result.Should().Throw<JsonException>();
    }
}