using System.Text.Json;
using System.Text.Json.Serialization;
using Code.Types.Collections;

namespace Code.Infrastructure.Json.ConvertFactories;

public sealed class ReadOnlyArrayWithEqualityJsonConvertFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) 
        => JsonSerializationHelper.CanConvert(typeToConvert, typeof(ReadOnlyArrayWithEquality<>));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializationHelper.CreateConverter(JsonSerializationHelper.GetGenericTypeWithOneType(typeToConvert, typeof(ReadOnlyArrayWithEqualityJsonConverter<>)));

    private sealed class ReadOnlyArrayWithEqualityJsonConverter<T> : JsonConverter<ReadOnlyArrayWithEquality<T>>
    {
        public override bool HandleNull => true;

        public override ReadOnlyArrayWithEquality<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => JsonSerializationHelper.ReadArray<T, ReadOnlyArrayWithEquality<T>>(ref reader, options, static arg => new ReadOnlyArrayWithEquality<T>(arg), static () => ReadOnlyArrayWithEquality<T>.Empty());

        public override void Write(Utf8JsonWriter writer, ReadOnlyArrayWithEquality<T> value, JsonSerializerOptions options) 
            => JsonSerializationHelper.WriteEnumerable(writer, value, options);
    }
}