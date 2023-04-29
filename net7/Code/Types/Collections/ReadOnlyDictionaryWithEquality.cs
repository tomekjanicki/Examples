using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyDictionaryWithEquality<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>, IEquatable<ReadOnlyDictionaryWithEquality<TKey, TValue>>
    where TKey : notnull
{
    public ReadOnlyDictionaryWithEquality()
        : base(new Dictionary<TKey, TValue>())
    {
    }

    public ReadOnlyDictionaryWithEquality(Dictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
    }

    public static ReadOnlyDictionaryWithEquality<TKey, TValue> Empty() => ObjectCache.GetOrCreate<ReadOnlyDictionaryWithEquality<TKey, TValue>>();

    public bool Equals(ReadOnlyDictionaryWithEquality<TKey, TValue>? other) => EqualityHelper<ReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, other, static (me, oth) => AreEqual(me, oth));

    public override bool Equals(object? obj) => EqualityHelper<ReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, obj, (me, oth) => me.Equals(oth));

    public override int GetHashCode() => HashCodeHelper<TKey, TValue>.GetHashCode(Dictionary);

    public static bool operator !=(ReadOnlyDictionaryWithEquality<TKey, TValue> left, ReadOnlyDictionaryWithEquality<TKey, TValue> right) => !(left == right);

    public static bool operator ==(ReadOnlyDictionaryWithEquality<TKey, TValue> left, ReadOnlyDictionaryWithEquality<TKey, TValue> right) => left.Equals(right);
}