using System.Text.Json;
using System.Text.Json.Serialization;
using Code.Types.Collections;

namespace Code.Infrastructure.Json.ConvertFactories;

public sealed class ReadOnlyDictionaryJsonConvertFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => JsonSerializationHelper.CanConvert(typeToConvert, typeof(ReadOnlyDictionary<,>));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializationHelper.CreateConverter(JsonSerializationHelper.GetGenericTypeWithTwoTypes(typeToConvert, typeof(ReadOnlyDictionaryJsonConverter<,>)));

    private sealed class ReadOnlyDictionaryJsonConverter<TKey, TValue> : JsonConverter<ReadOnlyDictionary<TKey, TValue>>
        where TKey : notnull
    {
        public override ReadOnlyDictionary<TKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => JsonSerializationHelper.ReadDictionary<TKey, TValue, ReadOnlyDictionary<TKey, TValue>>(ref reader, options,
                static values => new ReadOnlyDictionary<TKey, TValue>(values), static () => ReadOnlyDictionary<TKey, TValue>.Empty());

        public override void Write(Utf8JsonWriter writer, ReadOnlyDictionary<TKey, TValue> collection, JsonSerializerOptions options) 
            => JsonSerializationHelper.WriteDictionary(writer, collection, options);
    }
}