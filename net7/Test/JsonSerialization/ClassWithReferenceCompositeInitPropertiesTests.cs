using System.Text.Json;
using Code.Infrastructure.Json;
using Code.Models;
using FluentAssertions;

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
}