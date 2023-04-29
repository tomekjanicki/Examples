using Code.Types.Collections.Abstractions;

namespace Code.Types.Collections;

public sealed class ReadOnlyArray<T> : BaseReadOnlyArray<T>
{
    public ReadOnlyArray(IEnumerable<T> enumerable)
        : base(enumerable)
    {
    }

    public ReadOnlyArray(T[] array)
        : base(array)
    {
    }

    public ReadOnlyArray(T item)
        : base(item)
    {
    }

    public ReadOnlyArray()
        : base(System.Array.Empty<T>())
    {
    }

    public static ReadOnlyArray<T> Empty() => ObjectCache.GetOrCreate<ReadOnlyArray<T>>();
}