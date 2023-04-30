using Code.Types.Collections;
using FluentAssertions;

namespace Test.Types.Collections;

public class ReadOnlyArrayWithIsEquivalentTests
{
    [Fact]
    public void TwoClassesWithTheSameContent()
    {
        var first = new ReadOnlyArrayWithIsEquivalent<int>(new[] { 1, 2, 3, 1, 2, 3 });
        var second = new ReadOnlyArrayWithIsEquivalent<int>(new[] { 1, 1, 2, 2, 3, 3 });
        first.Equals(second).Should().BeTrue();
        (first == second).Should().BeTrue();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }
}