using System.Text.Json;
using System.Text.Json.Serialization;

namespace Code.Infrastructure.Json.ConvertFactories;

public sealed class ReadOnlyCollectionJsonConvertFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert) 
        => JsonSerializationHelper.CanConvert(typeToConvert, typeof(IReadOnlyCollection<>));

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        => JsonSerializationHelper.CreateConverter(JsonSerializationHelper.GetGenericTypeWithOneType(typeToConvert, typeof(ReadOnlyCollectionJsonConverter<>)));

    private sealed class ReadOnlyCollectionJsonConverter<T> : JsonConverter<IReadOnlyCollection<T>>
    {
        public override bool HandleNull => true;

        public override IReadOnlyCollection<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) 
            => JsonSerializationHelper.ReadArray<T, IReadOnlyCollection<T>>(ref reader, options, static arg => arg, static () => Array.Empty<T>());

        public override void Write(Utf8JsonWriter writer, IReadOnlyCollection<T> value, JsonSerializerOptions options) 
            => JsonSerializationHelper.WriteEnumerable(writer, value, options);
    }
}