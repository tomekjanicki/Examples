using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyDictionaryWithEquality<TKey, TValue> : BaseEquatableReadOnlyDictionary<TKey, TValue>, IEquatable<ReadOnlyDictionaryWithEquality<TKey, TValue>>
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

    protected override int GetHashCodeInt() => HashCodeHelper<TKey, TValue>.GetHashCode(Dictionary);

    protected override bool EqualsInt(object? obj) => EqualityHelper<ReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, obj, (me, oth) => me.Equals(oth));
}