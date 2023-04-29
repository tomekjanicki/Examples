using Code.Types.Collections.Abstractions;
using OneOf.Types;
using OneOf;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyUniqueCollection<T> : BaseReadOnlyUniqueCollection<T>
{
    private NonEmptyReadOnlyUniqueCollection(IEnumerable<T> items)
        : base(items)
    {
    }

    public NonEmptyReadOnlyUniqueCollection(T item)
        : base(item)
    {
    }

    public static OneOf<NonEmptyReadOnlyUniqueCollection<T>, Error<string>> TryCreate(IReadOnlyCollection<T> items)
        => !items.Any() ? new Error<string>("At least one item required.") : new NonEmptyReadOnlyUniqueCollection<T>(items);
}