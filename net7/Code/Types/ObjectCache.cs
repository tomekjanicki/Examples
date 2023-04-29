namespace Code.Types;

public static class ObjectCache
{
    private static readonly Dictionary<Type, object> EmptyObjects = new();

    public static T GetOrCreate<T>()
        where T : class, new()
    {
        var type = typeof(T);

        return (T)PoolHelper.GetOrCreate(EmptyObjects, type, false, static _ => new T());
    }
}