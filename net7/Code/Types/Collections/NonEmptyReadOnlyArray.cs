using Code.Types.Collections.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyArray<T> : BaseReadOnlyArray<T>
{
    private const string AtLeastOneItemRequired = "At least one item required.";

    public NonEmptyReadOnlyArray(T[] array) 
        : base(array)
    {
        if (!array.Any())
        {
            throw new InvalidOperationException(AtLeastOneItemRequired);
        }
    }

    public NonEmptyReadOnlyArray(T item) 
        : base(item)
    {
    }

    public static OneOf<NonEmptyReadOnlyArray<T>, Error<string>> TryCreate(T[] items) => 
        !items.Any() ? new Error<string>(AtLeastOneItemRequired) : new NonEmptyReadOnlyArray<T>(items);
}