using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Models;
using FluentAssertions;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithConstructorAndReadOnlyCollectionPropertyTests
{
    [Fact]
    public void Serialize()
    {
        var obj = new ClassWithConstructorAndReadOnlyCollectionProperty(Array.Empty<int>());
        var jsonString = JsonSerializerWrapper.Serialize(obj);
        jsonString.Should().Be(@"{""Items"":[]}");
    }

    [Fact]
    public void DeserializeWithObject()
    {
        const string value = @"{""Items"":[]}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithNull()
    {
        const string value = @"{""Items"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithMissing()
    {
        //not good
        const string value = @"{}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndReadOnlyCollectionProperty(Array.Empty<int>());
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(serialized);
        destObj.Items.Count.Should().Be(0);
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeForceNull()
    {
        var sourceObj = new ClassWithConstructorAndReadOnlyCollectionPropertyTmp(null);
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(serialized);
        destObj.Items.Count.Should().Be(0);
    }

    [MessagePackObject]
    public sealed class ClassWithConstructorAndReadOnlyCollectionPropertyTmp
    {
        [SerializationConstructor]
        public ClassWithConstructorAndReadOnlyCollectionPropertyTmp(IReadOnlyCollection<int>? items)
        {
            Items = items;
        }

        [Key(0)]
        public IReadOnlyCollection<int>? Items { get; }
    }
}