using Code.Types.Collections.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>, IEquatable<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>
    where TKey : notnull
{
    private NonEmptyReadOnlyDictionaryWithEquality(Dictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
    }

    public static OneOf<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>, Error<string>> TryCreate(Dictionary<TKey, TValue> dictionary)
        => !dictionary.Any() ? new Error<string>("At least one item required.") : new NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>(dictionary);

    public bool Equals(NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>? other) => EqualityHelper<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, other, static (me, oth) => AreEqual(me, oth));

    public override bool Equals(object? obj) => EqualityHelper<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, obj, (me, oth) => me.Equals(oth));

    public override int GetHashCode() => HashCodeHelper<TKey, TValue>.GetHashCode(Dictionary);

    public static bool operator !=(NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> left, NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> right) => !(left == right);

    public static bool operator ==(NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> left, NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> right) => left.Equals(right);

}