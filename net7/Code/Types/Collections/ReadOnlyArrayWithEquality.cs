using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyArrayWithEquality<T> : BaseReadOnlyArray<T>, IEquatable<ReadOnlyArrayWithEquality<T>>
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

    public override bool Equals(object? obj) => EqualityHelper<ReadOnlyArrayWithEquality<T>>.Equals(this, obj, (me, oth) => me.Equals(oth));

    public override int GetHashCode() => HashCodeHelper<T>.GetHashCode(Array);

    public static bool operator !=(ReadOnlyArrayWithEquality<T> left, ReadOnlyArrayWithEquality<T> right) => !(left == right);

    public static bool operator ==(ReadOnlyArrayWithEquality<T> left, ReadOnlyArrayWithEquality<T> right) => left.Equals(right);
}