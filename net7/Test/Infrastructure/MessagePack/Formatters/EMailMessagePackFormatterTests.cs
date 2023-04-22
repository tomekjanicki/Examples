using Code.StronglyTypeIds;
using FluentAssertions;
using MessagePack;

namespace Test.Infrastructure.MessagePack.Formatters;

public class EMailMessagePackFormatterTests
{
    [Fact]
    public void ShouldSerializeAndDeserializeWithValidObject()
    {
        var eMail = (EMail)"test@test.com";
        var result = MessagePackSerializer.Serialize(eMail);
        var obj = MessagePackSerializer.Deserialize<EMail>(result);
        obj.Value.Should().Be(eMail.Value);
    }

    [Fact]
    public void NullableShouldSerializeAndDeserialize()
    {
        var eMail = (EMail?)null;
        var result = MessagePackSerializer.Serialize(eMail);
        var obj = MessagePackSerializer.Deserialize<EMail?>(result);
        obj.Should().BeNull();
    }

    [Fact]
    public void ShouldNotDeserializeFromInvalidValue()
    {
        var result = MessagePackSerializer.Serialize("not valid");
        var func = () => MessagePackSerializer.Deserialize<EMail?>(result);
        func.Should().Throw<MessagePackSerializationException>();
    }
}