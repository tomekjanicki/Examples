using Code.Types.Collections.Abstractions;
using OneOf.Types;
using OneOf;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyArrayWithEquality<T> : BaseEquatableReadOnlyArray<T>, IEquatable<NonEmptyReadOnlyArrayWithEquality<T>>
{
    private NonEmptyReadOnlyArrayWithEquality(T[] array)
        : base(array)
    {
    }

    public NonEmptyReadOnlyArrayWithEquality(T item)
        : base(item)
    {
    }

    public bool Equals(NonEmptyReadOnlyArrayWithEquality<T>? other) => EqualityHelper<NonEmptyReadOnlyArrayWithEquality<T>>.Equals(this, other, static (me, oth) => me.SequenceEqual(oth));

    protected override int GetHashCodeInt() => HashCodeHelper<T>.GetHashCode(Array);

    protected override bool EqualsInt(object? obj) => EqualityHelper<NonEmptyReadOnlyArrayWithEquality<T>>.Equals(this, obj, (me, oth) => me.Equals(oth));
    
    public static OneOf<NonEmptyReadOnlyArrayWithEquality<T>, Error<string>> TryCreate(T[] items) =>
        !items.Any() ? new Error<string>("At least one item required.") : new NonEmptyReadOnlyArrayWithEquality<T>(items);
}