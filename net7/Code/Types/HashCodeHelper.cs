namespace Code.Types;

public static class HashCodeHelper<T>
{
    private static readonly EqualityComparer<T> ElementComparer = EqualityComparer<T>.Default;

    public static int GetHashCode(IEnumerable<T> enumerable)
    {
        unchecked
        {
            return enumerable.Aggregate(17, static (current, element) => current * 31 + (element is null ? 0 : ElementComparer.GetHashCode(element)));
        }
    }
}

public static class HashCodeHelper<TKey, TValue>
    where TKey : notnull
{
    private static readonly EqualityComparer<TKey> KeyElementComparer = EqualityComparer<TKey>.Default;
    private static readonly EqualityComparer<TValue> ValueElementComparer = EqualityComparer<TValue>.Default;

    public static int GetHashCode(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
    {
        unchecked
        {
            return enumerable.Aggregate(17, static (current, element) => current * 31 + GetElementHashCode(element));
        }
    }

    private static int GetElementHashCode(KeyValuePair<TKey, TValue> item)
    {
        unchecked
        {
            var keyHashCode = KeyElementComparer.GetHashCode(item.Key);
            var valueHashCode = item.Value is null ? 0 : ValueElementComparer.GetHashCode(item.Value);

            return keyHashCode + valueHashCode;
        }
    }

}