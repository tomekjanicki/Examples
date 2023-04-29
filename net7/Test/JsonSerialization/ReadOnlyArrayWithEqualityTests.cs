using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Types.Collections;
using FluentAssertions;

namespace Test.JsonSerialization;

public sealed class ReadOnlyArrayWithEqualityTests
{
    [Fact]
    public void ShouldSerializeAndDeserialize()
    {
        const int value = 12;
        var array = new ReadOnlyArrayWithEquality<int>(value);
        var jsonString = JsonSerializerWrapper.Serialize(array);
        var deserializedArray = JsonSerializerWrapper.Deserialize<ReadOnlyArrayWithEquality<int>>(jsonString);
        deserializedArray.Should().NotBeNull();
        deserializedArray![0].Should().Be(value);
    }

    [Fact]
    public void MessagePackShouldSerializeAndDeserialize()
    {
        const int value = 12;
        var array = new ReadOnlyArrayWithEquality<int>(value);
        var bytes = MessagePackSerializerWrapper.Serialize(array);
        var deserializedArray = MessagePackSerializerWrapper.Deserialize<ReadOnlyArrayWithEquality<int>>(bytes);
        deserializedArray.Should().NotBeNull();
        deserializedArray[0].Should().Be(value);
    }

}