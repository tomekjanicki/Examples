using Code.Types.Collections.Abstractions;
using OneOf.Types;
using OneOf;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyArrayWithEquality<T> : BaseReadOnlyArray<T>, IEquatable<NonEmptyReadOnlyArrayWithEquality<T>>
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

    public override bool Equals(object? obj) => EqualityHelper<NonEmptyReadOnlyArrayWithEquality<T>>.Equals(this, obj, (me, oth) => me.Equals(oth));

    public override int GetHashCode() => HashCodeHelper<T>.GetHashCode(Array);

    public static bool operator !=(NonEmptyReadOnlyArrayWithEquality<T> left, NonEmptyReadOnlyArrayWithEquality<T> right) => !(left == right);

    public static bool operator ==(NonEmptyReadOnlyArrayWithEquality<T> left, NonEmptyReadOnlyArrayWithEquality<T> right) => left.Equals(right);

    public static OneOf<NonEmptyReadOnlyArrayWithEquality<T>, Error<string>> TryCreate(T[] items) =>
        !items.Any() ? new Error<string>("At least one item required.") : new NonEmptyReadOnlyArrayWithEquality<T>(items);
}