using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Types.Collections;
using FluentAssertions;

namespace Test.JsonSerialization;

public sealed class ReadOnlyArrayTests
{
    [Fact]
    public void ShouldSerializeAndDeserialize()
    {
        const int value = 12;
        var array = new ReadOnlyArray<int>(value);
        var jsonString = JsonSerializerWrapper.Serialize(array);
        var deserializedArray = JsonSerializerWrapper.Deserialize<ReadOnlyArray<int>>(jsonString);
        deserializedArray.Should().NotBeNull();
        deserializedArray![0].Should().Be(value);
    }

    [Fact]
    public void MessagePackShouldSerializeAndDeserialize()
    {
        const int value = 12;
        var array = new ReadOnlyArray<int>(value);
        var bytes = MessagePackSerializerWrapper.Serialize(array);
        var deserializedArray = MessagePackSerializerWrapper.Deserialize<ReadOnlyArray<int>>(bytes);
        deserializedArray.Should().NotBeNull();
        deserializedArray[0].Should().Be(value);
    }
}