using Code;
using FluentAssertions;

namespace Test;

public class ExtensionsTests
{
    [Fact]
    public void IsEquivalentShouldBehaveAsExpected()
    {
        var first = new[] { 1, 2, 3, 2, 1 };
        var second = new[] { 1, 1, 3, 2, 2 };
        var result = first.IsEquivalent(second);
        result.Should().BeTrue();
    }
}