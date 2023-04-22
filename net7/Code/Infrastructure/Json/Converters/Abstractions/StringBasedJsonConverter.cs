using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.Json.Converters.Abstractions;

public abstract class StringBasedJsonConverter<T> : JsonConverter<T>
    where T : class
{
    private readonly Func<string?, OneOf<T, Error<string>>> _constructorFactory;
    private readonly Func<T, string> _valueProvider;

    protected StringBasedJsonConverter(Func<string?, OneOf<T, Error<string>>> constructorFactory, Func<T, string> valueProvider)
    {
        _constructorFactory = constructorFactory;
        _valueProvider = valueProvider;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = _constructorFactory(GetStringValue(ref reader));

        return result.IsT0 ? result.AsT0 : throw new JsonException(result.AsT1.Value);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var valueConverter = (JsonConverter<string>)options.GetConverter(typeof(string));
        valueConverter.Write(writer, _valueProvider(value), options);
    }

    private static string? GetStringValue(ref Utf8JsonReader reader) => reader.TokenType == JsonTokenType.String ? reader.GetString() : null;
}