using Code.Types.Collections.Abstractions;
using OneOf;
using OneOf.Types;

namespace Code.Types.Collections;

public sealed class NonEmptyReadOnlyDictionary<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    private const string AtLeastOneItemRequired = "At least one item required.";

    public NonEmptyReadOnlyDictionary(Dictionary<TKey, TValue> dictionary) 
        : base(dictionary)
    {
        if (!dictionary.Any())
        {
            throw new InvalidOperationException(AtLeastOneItemRequired);
        }
    }

    public static OneOf<NonEmptyReadOnlyDictionary<TKey, TValue>, Error<string>> TryCreate(Dictionary<TKey, TValue> dictionary)
        => !dictionary.Any() ? new Error<string>(AtLeastOneItemRequired) : new NonEmptyReadOnlyDictionary<TKey, TValue>(dictionary);
}