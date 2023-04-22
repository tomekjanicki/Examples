using Code.StronglyTypeIds;
using FluentAssertions;

namespace Test.StronglyTypeIds;

public class DeviceIdTests
{
    [Fact]
    public void Default()
    {
        DeviceId deviceId = default;
        var action1 = () => (int)deviceId;
        action1.Should().Throw<InvalidOperationException>();
        var action2 = () => deviceId.Value;
        action2.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ParameterLessConstructor()
    {
        var deviceId = new DeviceId();
        deviceId.Value.Should().Be(1);
    }

    [Fact]
    public void TryCreateSuccess()
    {
        var result = DeviceId.TryCreate(2);
        result.IsT0.Should().BeTrue();
        result.AsT0.Value.Should().Be(2);
    }

    [Fact]
    public void TryCreateNull()
    {
        var result = DeviceId.TryCreate(null);
        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Be(DeviceId.NullValueError);
    }

    [Fact]
    public void TryCreateZero()
    {
        var result = DeviceId.TryCreate(0);
        result.IsT1.Should().BeTrue();
        result.AsT1.Value.Should().Be(DeviceId.ValueNotGreaterThanZeroError);
    }

    [Fact]
    public void ImplicitConversion()
    {
        var result = DeviceId.TryCreate(5);
        (result.AsT0 == 5).Should().BeTrue();
    }

    [Fact]
    public void ExplicitConversionValidValue()
    {
        var deviceId = (DeviceId)5;

        deviceId.Value.Should().Be(5);
    }

    [Fact]
    public void ExplicitConversionInvalidValue()
    {
        var func = () => (DeviceId)0;

        func.Should().Throw<InvalidCastException>();
    }
}