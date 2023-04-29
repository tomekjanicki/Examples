using System.Collections.Concurrent;
using System.Reflection;
using Code.Infrastructure.MessagePack.Formatters;
using Code.Types.Collections;
using MessagePack;
using MessagePack.Formatters;

namespace Code.Infrastructure.MessagePack;

public static class MessagePackSerializationHelper
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
        if (!Dictionary.TryGetValue(type.GetGenericTypeDefinition(), out var constructor))
        {
            return null;
        }

        var instance = constructor(type);
        FormatterMap.TryAdd(type, instance);

        return (IMessagePackFormatter<T>)instance;
    }

    private static readonly Dictionary<Type, Func<Type, object>> Dictionary;

    static MessagePackSerializationHelper()
    {
        Dictionary = new Dictionary<Type, Func<Type, object>>
        {
            { typeof(IReadOnlyCollection<>), static type => GetObject(GetGenericTypeWithOneType(type, typeof(ReadOnlyCollectionMessagePackFormatter<>))) },
            { typeof(ReadOnlyArray<>), static type => GetObject(GetGenericTypeWithOneType(type, typeof(ReadOnlyArrayMessagePackFormatter<>))) },
            { typeof(ReadOnlyDictionary<,>), static type => GetObject(GetGenericTypeWithTwoTypes(type, typeof(ReadOnlyDictionaryMessagePackFormatter<,>))) }
        };
    }

    private static Type GetGenericTypeWithOneType(Type typeToConvert, Type converterOpenType)
    {
        var types = typeToConvert.GetGenericArguments();

        return converterOpenType.MakeGenericType(types[0]);
    }

    private static Type GetGenericTypeWithTwoTypes(Type typeToConvert, Type converterOpenType)
    {
        var types = typeToConvert.GetGenericArguments();

        return converterOpenType.MakeGenericType(types[0], types[1]);
    }

    private static object GetObject(Type type)
    {
        return Activator.CreateInstance(
            type,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: Array.Empty<object>(),
            culture: null)!;
    }

    private static void Serialize<T>(ref MessagePackWriter writer, T? value, MessagePackSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNil();

            return;
        }
        options.Resolver.GetFormatterWithVerify<T>().Serialize(ref writer, value, options);
    }

    public static void SerializeArray<T>(ref MessagePackWriter writer, IEnumerable<T>? value, MessagePackSerializerOptions options) 
        => Serialize(ref writer, value, options);

    public static void SerializeDictionary<TKey, TValue>(ref MessagePackWriter writer, IReadOnlyDictionary<TKey, TValue>? value, MessagePackSerializerOptions options) 
        => Serialize(ref writer, value, options);

    private static TReturn Deserialize<T, TReturn>(ref MessagePackReader reader, MessagePackSerializerOptions options,
        Func<T, TReturn> constructorFunc, Func<TReturn> emptyFunc)
        => reader.IsNil ? emptyFunc() : constructorFunc(options.Resolver.GetFormatterWithVerify<T>().Deserialize(ref reader, options));

    public static TReturn DeserializeArray<T, TReturn>(ref MessagePackReader reader, MessagePackSerializerOptions options,
        Func<T[], TReturn> constructorFunc, Func<TReturn> emptyFunc)
        => Deserialize(ref reader, options, constructorFunc, emptyFunc);

    public static TReturn DeserializeDictionary<TKey, TValue, TReturn>(ref MessagePackReader reader, MessagePackSerializerOptions options,
        Func<Dictionary<TKey, TValue>, TReturn> constructorFunc, Func<TReturn> emptyFunc)
        where TKey : notnull
        => Deserialize(ref reader, options, constructorFunc, emptyFunc);
}