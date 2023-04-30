using Code.Types.Collections.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue> : BaseEquatableReadOnlyDictionary<TKey, TValue>, IEquatable<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>
    where TKey : notnull
{
    private NonEmptyReadOnlyDictionaryWithEquality(Dictionary<TKey, TValue> dictionary)
        : base(dictionary)
    {
    }

    public static OneOf<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>, Error<string>> TryCreate(Dictionary<TKey, TValue> dictionary)
        => !dictionary.Any() ? new Error<string>("At least one item required.") : new NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>(dictionary);

    public bool Equals(NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>? other) => EqualityHelper<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, other, static (me, oth) => AreEqual(me, oth));

    protected override int GetHashCodeInt() => HashCodeHelper<TKey, TValue>.GetHashCode(Dictionary);

    protected override bool EqualsInt(object? obj) => EqualityHelper<NonEmptyReadOnlyDictionaryWithEquality<TKey, TValue>>.Equals(this, obj, (me, oth) => me.Equals(oth));
}