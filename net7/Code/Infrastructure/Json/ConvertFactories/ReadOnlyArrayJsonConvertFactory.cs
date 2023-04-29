using System.Text.Json;
using System.Text.Json.Serialization;
using Code.Types.Collections;

namespace Code.Infrastructure.Json.ConvertFactories;

public sealed class ReadOnlyArrayJsonConvertFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) 
        => JsonSerializationHelper.CanConvert(typeToConvert, typeof(ReadOnlyArray<>));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) 
        => JsonSerializationHelper.CreateConverter(JsonSerializationHelper.GetGenericTypeWithOneType(typeToConvert, typeof(ReadOnlyArrayJsonConverter<>)));

    private sealed class ReadOnlyArrayJsonConverter<T> : JsonConverter<ReadOnlyArray<T>>
    {
        public override bool HandleNull => true;

        public override ReadOnlyArray<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => JsonSerializationHelper.ReadArray<T, ReadOnlyArray<T>>(ref reader, options, static arg => new ReadOnlyArray<T>(arg), static () => ReadOnlyArray<T>.Empty());

        public override void Write(Utf8JsonWriter writer, ReadOnlyArray<T> value, JsonSerializerOptions options) 
            => JsonSerializationHelper.WriteEnumerable(writer, value, options);
    }
}