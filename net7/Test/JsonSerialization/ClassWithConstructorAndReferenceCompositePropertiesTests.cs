using Code.Models;
using FluentAssertions;
using Code.Infrastructure.Json;
using Code.StronglyTypeIds;
using MessagePack;
using Code.Infrastructure.MessagePack;

namespace Test.JsonSerialization;

public class ClassWithConstructorAndReferenceCompositePropertiesTests
{
    [Fact]
    public void DeserializeWithAllProperties()
    {
        const string jsonString = @"{""EMail"": ""test@test.com"",""OptionalEmail"":null}";
        var result = JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        result.Should().NotBeNull();
        result!.EMail.Value.Should().Be("test@test.com");
        result.OptionalEmail.Should().BeNull();
    }

    [Fact]
    public void DeserializeWithForceNull()
    {
        const string jsonString = @"{""EMail"": null,""OptionalEmail"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void DeserializeWithMissingProperty()
    {
        const string jsonString = @"{""OptionalEmail"":null}";
        var func = () => JsonSerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(jsonString);
        func.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void MessagePackSerializeAndDeserialize()
    {
        var sourceObj = new ClassWithConstructorAndReferenceCompositeProperties((EMail)"test@test.com", null);
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var destObj = MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(serialized);
        destObj.EMail.Value.Should().Be("test@test.com");
        destObj.OptionalEmail.Should().BeNull();
    }

    [Fact]
    public void MessagePackSerializeAndDeserializeWithForceNull()
    {
        var sourceObj = new ClassWithConstructorAndReferenceCompositePropertiesTemp(null, null);
        var serialized = MessagePackSerializerWrapper.Serialize(sourceObj);
        var func = () => MessagePackSerializerWrapper.Deserialize<ClassWithConstructorAndReferenceCompositeProperties>(serialized);

        func.Should().Throw<MessagePackSerializationException>().WithInnerException<ArgumentNullException>();
    }

    [MessagePackObject]
    public sealed class ClassWithConstructorAndReferenceCompositePropertiesTemp
    {
        [SerializationConstructor]
        public ClassWithConstructorAndReferenceCompositePropertiesTemp(EMail? eMail, EMail? optionalEmail)
        {
            EMail = eMail;
            OptionalEmail = optionalEmail;
        }

        [Key(0)]
        public EMail? EMail { get; }

        [Key(1)]
        public EMail? OptionalEmail { get; }
    }
}