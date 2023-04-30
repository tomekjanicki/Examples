using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyArrayWithIsEquivalent<T> : BaseEquatableReadOnlyArray<T>, IEquatable<ReadOnlyArrayWithIsEquivalent<T>>
{
    public ReadOnlyArrayWithIsEquivalent(IEnumerable<T> enumerable)
        : base(enumerable)
    {
    }

    public ReadOnlyArrayWithIsEquivalent(T[] array)
        : base(array)
    {
    }

    public ReadOnlyArrayWithIsEquivalent(T item)
        : base(item)
    {
    }

    public ReadOnlyArrayWithIsEquivalent()
        : base(System.Array.Empty<T>())
    {
    }

    public static ReadOnlyArrayWithIsEquivalent<T> Empty() => ObjectCache.GetOrCreate<ReadOnlyArrayWithIsEquivalent<T>>();

    public bool Equals(ReadOnlyArrayWithIsEquivalent<T>? other) => EqualityHelper<ReadOnlyArrayWithIsEquivalent<T>>.Equals(this, other, static (me, oth) => me.IsEquivalent(oth));

    protected override int GetHashCodeInt() => HashCodeHelper<T>.GetHashCode(Array.Order());

    protected override bool EqualsInt(object? obj) => EqualityHelper<ReadOnlyArrayWithIsEquivalent<T>>.Equals(this, obj, (me, oth) => me.Equals(oth));
}