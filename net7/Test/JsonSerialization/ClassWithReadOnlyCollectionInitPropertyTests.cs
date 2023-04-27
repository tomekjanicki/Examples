using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Models;
using FluentAssertions;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithReadOnlyCollectionInitPropertyTests
{
    [Fact]
    public void Serialize()
    {
        var obj = new ClassWithReadOnlyCollectionInitProperty { Items = Array.Empty<int>() };
        var jsonString = JsonSerializerWrapper.Serialize(obj);
        jsonString.Should().Be(@"{""Items"":[]}");
    }

    [Fact]
    public void DeserializeWithObject()
    {
        const string value = @"{""Items"":[]}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithNull()
    {
        const string value = @"{""Items"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithMissing()
    {
        const string value = @"{}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithReadOnlyCollectionInitProperty { Items = Array.Empty<int>() };
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(serialized);
        destObj.Items.Count.Should().Be(0);
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeForceNull()
    {
        var sourceObj = new ClassWithReadOnlyCollectionInitPropertyTmp();
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(serialized);
        destObj.Items.Count.Should().Be(0);
    }

    [MessagePackObject]
    public sealed class ClassWithReadOnlyCollectionInitPropertyTmp
    {
        [Key(0)]
        public IReadOnlyCollection<int>? Items { get; init; }
    }
}