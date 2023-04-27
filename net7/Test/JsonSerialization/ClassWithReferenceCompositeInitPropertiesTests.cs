using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Infrastructure.MessagePack;
using Code.Models;
using Code.StronglyTypeIds;
using FluentAssertions;
using MessagePack;

namespace Test.JsonSerialization;

public class ClassWithReferenceCompositeInitPropertiesTests
{
    [Fact]
    public void DeserializeWithAllProperties()
    {
        const string jsonString = @"{""EMail"": ""test@test.com"",""OptionalEmail"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithReferenceCompositeInitProperties>(jsonString);
        result.Should().NotBeNull();
        result!.EMail.Value.Should().Be("test@test.com");
        result.OptionalEmail.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""EMail"": null,""OptionalEmail"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithReferenceCompositeInitProperties>(jsonString);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        const string jsonString = @"{""OptionalEmail"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithReferenceCompositeInitProperties>(jsonString);
        func.Should().Throw<JsonException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithReferenceCompositeInitProperties { EMail = (EMail)"test@test.com" };
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithReferenceCompositeInitProperties>(serialized);
        destObj.EMail.Value.Should().Be("test@test.com");
        destObj.OptionalEmail.Should().BeNull();
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeWithForceNull()
    {
        var sourceObj = new ClassWithReferenceCompositeInitPropertiesTemp();
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var func = () => MessagePackSerializerWrapper.Deserialize<ClassWithReferenceCompositeInitProperties>(serialized);

        func.Should().Throw<MessagePackSerializationException>().WithInnerException<ArgumentNullException>();
    }

    [MessagePackObject]
    public sealed class ClassWithReferenceCompositeInitPropertiesTemp
    {
        [Key(0)]
        public EMail? EMail { get; init; }

        [Key(1)]
        public EMail? OptionalEmail { get; init; }
    }
}