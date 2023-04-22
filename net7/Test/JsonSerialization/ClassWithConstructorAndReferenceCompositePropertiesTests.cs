using Code.Models;
using FluentAssertions;
using Code.Infrastructure.Json;
using Code.StronglyTypeIds;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithConstructorAndReferenceCompositePropertiesTests
{
    [Fact]
    public void DeserializeWithAllProperties()
    {
        const string jsonString = @"{""EMail"": ""test@test.com"",""OptionalEmail"":null}";
        var result = SerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        result!.EMail.Value.Should().Be("test@test.com");
        result.OptionalEmail.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""EMail"": null,""OptionalEmail"":null}";
        var func = () => SerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        const string jsonString = @"{""OptionalEmail"":null}";
        var func = () => SerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndReferenceCompositeProperties((EMail)"test@test.com", null);
        var serialized = MessagePackSerializer.Serialize(sourceObj);
        var destObj = MessagePackSerializer.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(serialized);
        destObj.EMail.Value.Should().Be("test@test.com");
        destObj.OptionalEmail.Should().BeNull();
    }
}