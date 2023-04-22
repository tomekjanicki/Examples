using Code.Infrastructure.Json;
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
        var jsonString = SerializerWrapper.Serialize(obj);
        jsonString.Should().Be(@"{""Items"":[]}");
    }

    [Fact]
    public void DeserializeWithObject()
    {
        const string value = @"{""Items"":[]}";
        var result = SerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithNull()
    {
        const string value = @"{""Items"":null}";
        var result = SerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithMissing()
    {
        //not good
        const string value = @"{}";
        var func = () => SerializerWrapper.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(value);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndReadOnlyCollectionProperty(Array.Empty<int>());
        var serialized = MessagePackSerializer.Serialize(sourceObj);
        var destObj = MessagePackSerializer.Deserialize<ClassWithConstructorAndReadOnlyCollectionProperty>(serialized);
        destObj.Items.Count.Should().Be(0);
    }
}