using System.Collections.Concurrent;
using System.Reflection;
using Code.Infrastructure.MessagePack.Formatters;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack.Resolvers;

public static class CustomFormatterResolverGetHelper
{
    private static readonly ConcurrentDictionary<Type, object> FormatterMap = new();

    public static IMessagePackFormatter<T>? GetFormatter<T>()
    {
        var type = typeof(T);
        if (FormatterMap.TryGetValue(type, out var obj))
        {
            return (IMessagePackFormatter<T>)obj;
        }
        if (!type.IsGenericType)
        {
            return null;
        }
        if (type.GetGenericTypeDefinition() != typeof(IReadOnlyCollection<>))
        {
            return null;
        }
        var types = type.GetGenericArguments();
        var formatterType = typeof(ReadOnlyCollectionMessagePackFormatter<>).MakeGenericType(types[0]);
        var instance = Activator.CreateInstance(
            formatterType,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: Array.Empty<object>(),
            culture: null)!;
        FormatterMap.TryAdd(type, instance);

        return (IMessagePackFormatter<T>)instance;
    }
}