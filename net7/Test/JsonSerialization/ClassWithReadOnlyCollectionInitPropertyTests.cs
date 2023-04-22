using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Models;
using FluentAssertions;

namespace Test.JsonSerialization;

public class ClassWithReadOnlyCollectionInitPropertyTests
{
    [Fact]
    public void Serialize()
    {
        var obj = new ClassWithReadOnlyCollectionInitProperty { Items = Array.Empty<int>() };
        var jsonString = SerializerWrapper.Serialize(obj);
        jsonString.Should().Be(@"{""Items"":[]}");
    }

    [Fact]
    public void DeserializeWithObject()
    {
        const string value = @"{""Items"":[]}";
        var result = SerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithNull()
    {
        const string value = @"{""Items"":null}";
        var result = SerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        result.Should().NotBeNull();
        result!.Items.Count.Should().Be(0);
    }

    [Fact]
    public void DeserializeWithMissing()
    {
        const string value = @"{}";
        var func = () => SerializerWrapper.Deserialize<ClassWithReadOnlyCollectionInitProperty>(value);
        func.Should().Throw<JsonException>();
    }
}