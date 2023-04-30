using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyArrayWithEquality<T> : BaseEquatableReadOnlyArray<T>, IEquatable<ReadOnlyArrayWithEquality<T>>
{
    public ReadOnlyArrayWithEquality(IEnumerable<T> enumerable) 
        : base(enumerable)
    {
    }

    public ReadOnlyArrayWithEquality(T[] array) 
        : base(array)
    {
    }

    public ReadOnlyArrayWithEquality(T item) 
        : base(item)
    {
    }

    public ReadOnlyArrayWithEquality()
        : base(System.Array.Empty<T>())
    {
    }

    public static ReadOnlyArrayWithEquality<T> Empty() => ObjectCache.GetOrCreate<ReadOnlyArrayWithEquality<T>>();

    public bool Equals(ReadOnlyArrayWithEquality<T>? other) => EqualityHelper<ReadOnlyArrayWithEquality<T>>.Equals(this, other, static (me, oth) => me.SequenceEqual(oth));

    protected override int GetHashCodeInt() => HashCodeHelper<T>.GetHashCode(Array);

    protected override bool EqualsInt(object? obj) => EqualityHelper<ReadOnlyArrayWithEquality<T>>.Equals(this, obj, (me, oth) => me.Equals(oth));
}