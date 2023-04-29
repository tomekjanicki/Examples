using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Code.Infrastructure.Json;

public static class JsonSerializationHelper
{
    public static bool CanConvert(Type typeToConvert, Type converterOpenType) => typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == converterOpenType;

    public static Type GetGenericTypeWithOneType(Type typeToConvert, Type converterOpenType)
    {
        var types = typeToConvert.GetGenericArguments();

        return converterOpenType.MakeGenericType(types[0]);
    }

    public static Type GetGenericTypeWithTwoTypes(Type typeToConvert, Type converterOpenType)
    {
        var types = typeToConvert.GetGenericArguments();

        return converterOpenType.MakeGenericType(types[0], types[1]);
    }

    public static JsonConverter CreateConverter(Type type) =>
        (JsonConverter)Activator.CreateInstance(
            type,
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: Array.Empty<object>(),
            culture: null)!;

    public static TReturn ReadArray<TElement, TReturn>(ref Utf8JsonReader reader, JsonSerializerOptions options, Func<TElement[], TReturn> constructorFunc,
        Func<TReturn> emptyFunc) =>
        Read(ref reader, options, constructorFunc, emptyFunc);

    public static TReturn ReadDictionary<TKey, TValue, TReturn>(ref Utf8JsonReader reader, JsonSerializerOptions options,
        Func<Dictionary<TKey, TValue>, TReturn> constructorFunc, Func<TReturn> emptyFunc)
        where TKey : notnull =>
        Read(ref reader, options, constructorFunc, emptyFunc);

    private static TReturn Read<T, TReturn>(ref Utf8JsonReader reader, JsonSerializerOptions options, Func<T, TReturn> constructorFunc, Func<TReturn> emptyFunc)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return emptyFunc();
        }
        var valueType = typeof(T);
        var valueConverter = (JsonConverter<T>)options.GetConverter(valueType);
        var value = valueConverter.Read(ref reader, valueType, options)!;

        return constructorFunc(value);
    }

    public static void WriteEnumerable<T>(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options) 
        => Write(writer, value, options);

    public static void WriteDictionary<TKey, TValue>(Utf8JsonWriter writer, IReadOnlyDictionary<TKey, TValue> collection, JsonSerializerOptions options) 
        => Write(writer, collection, options);

    private static void Write<T>(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var valueConverter = (JsonConverter<T?>)options.GetConverter(typeof(T));
        valueConverter.Write(writer, value, options);
    }
}