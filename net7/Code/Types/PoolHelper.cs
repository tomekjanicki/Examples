namespace Code.Types;

public static class PoolHelper
{
    public static TValue GetOrCreate<TKey, TValue, TParam>(Dictionary<TKey, TValue> poolDictionary, TKey key, TParam param, Func<TParam, TValue> createNewFunc)
        where TKey : notnull
    {
        if (poolDictionary.TryGetValue(key, out var value))
        {
            return value;
        }
        lock (poolDictionary)
        {
            if (poolDictionary.TryGetValue(key, out value))
            {
                return value;
            }
            var instance = createNewFunc(param);
            poolDictionary.Add(key, instance);

            return instance;
        }
    }
}