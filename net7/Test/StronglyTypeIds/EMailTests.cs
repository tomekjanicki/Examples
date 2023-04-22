using Code.StronglyTypeIds;
using FluentAssertions;

namespace Test.StronglyTypeIds;

public class EMailTests
{
    [Fact]
    public void ValidEMail()
    {
        var result = EMail.TryCreate("test@test.com");

        result.IsT0.Should().BeTrue();
        result.AsT0.Value.Should().Be("test@test.com");
    }

    [Fact]
    public void NullValue()
    {
        var result = EMail.TryCreate(null);

        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Be(EMail.NullValueError);
    }

    [Fact]
    public void EmptyString()
    {
        var result = EMail.TryCreate(string.Empty);

        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Be(EMail.ValueNotValidEMailError);
    }

    [Fact]
    public void TooLongString()
    {
        var result = EMail.TryCreate(new string('a', 321));

        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Be(EMail.ValueGreaterThan320CharsError);
    }

    [Fact]
    public void ImplicitConversion()
    {
        var result = EMail.TryCreate("test@test.com");

        (result.AsT0 == "test@test.com").Should().BeTrue();
    }

    [Fact]
    public void ExplicitConversionValidValue()
    {
        var eMail = (EMail)"test@test.com";

        eMail.Value.Should().Be("test@test.com");
    }

    [Fact]
    public void ExplicitConversionInvalidValue()
    {
        var func = () => (EMail)"invalid value";

        func.Should().Throw<InvalidCastException>();
    }
}