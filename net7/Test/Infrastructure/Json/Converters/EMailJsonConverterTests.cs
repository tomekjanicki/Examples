using System.Text.Json;
using Code.Infrastructure.Json;
using Code.StronglyTypeIds;
using FluentAssertions;

namespace Test.Infrastructure.Json.Converters;

public class EMailJsonConverterTests
{
    [Fact]
    public void ShouldSerializeToJson()
    {
        var eMail = (EMail)"test@test.com";
        var result = SerializerWrapper.Serialize(eMail);
        result.Should().Be(@"""test@test.com""");
    }

    [Fact]
    public void ShouldDeserializeFromValidJsonString()
    {
        const string jsonString = @"""test@test.com""";
        var result = SerializerWrapper.Deserialize<EMail>(jsonString);
        result.Should().NotBeNull();
        result!.Value.Should().Be("test@test.com");
    }

    [Fact]
    public void ShouldNotDeserializeFromInvalidValueJsonString()
    {
        const string jsonString = @"""not valid""";
        var result = () => SerializerWrapper.Deserialize<EMail>(jsonString);
        result.Should().Throw<JsonException>();
    }
}