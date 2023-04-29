using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Types.Collections;
using FluentAssertions;

namespace Test.JsonSerialization;

public class ReadOnlyDictionaryTests
{
    [Fact]
    public void ShouldSerializeAndDeserialize()
    {
        const int value = 12;
        const int key = 17;
        var dictionary = new ReadOnlyDictionary<int, int>(new Dictionary<int, int> { { key, value } });
        var jsonString = JsonSerializerWrapper.Serialize(dictionary);
        var deserializedDictionary = JsonSerializerWrapper.Deserialize<ReadOnlyDictionary<int, int>>(jsonString);
        deserializedDictionary.Should().NotBeNull();
        var result = deserializedDictionary!.TryGetValue(key, out var val);
        result.Should().BeTrue();
        if (result)
        {
            val.Should().Be(value);
        }
    }

    [Fact]
    public void MessagePackShouldSerializeAndDeserialize()
    {
        const int value = 12;
        const int key = 17;
        var dictionary = new ReadOnlyDictionary<int, int>(new Dictionary<int, int> { { key, value } });
        var bytes = MessagePackSerializerWrapper.Serialize(dictionary);
        var deserializedDictionary = MessagePackSerializerWrapper.Deserialize<ReadOnlyDictionary<int, int>>(bytes);
        deserializedDictionary.Should().NotBeNull();
        var result = deserializedDictionary.TryGetValue(key, out var val);
        result.Should().BeTrue();
        if (result)
        {
            val.Should().Be(value);
        }
    }
}