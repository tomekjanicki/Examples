using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyDictionary<TKey, TValue> : BaseReadOnlyDictionary<TKey, TValue>
    where TKey : notnull
{
    public ReadOnlyDictionary()
        : base(new Dictionary<TKey, TValue>())
    {

    }

    public ReadOnlyDictionary(Dictionary<TKey, TValue> dictionary) 
        : base(dictionary)
    {
    }

    public static ReadOnlyDictionary<TKey, TValue> Empty() => ObjectCache.GetOrCreate<ReadOnlyDictionary<TKey, TValue>>();
}