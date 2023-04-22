using System.Text.Json;
using System.Text.Json.Serialization;
using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.Json.Converters.Abstractions;

public abstract class StructBasedJsonConvert<T, TPrimitiveType> : JsonConverter<T>
    where T : struct
    where TPrimitiveType : struct
{
    private readonly Func<TPrimitiveType?, OneOf<T, Error<string>>> _constructorFactory;
    private readonly Func<T, TPrimitiveType> _valueProvider;

    protected StructBasedJsonConvert(Func<TPrimitiveType?, OneOf<T, Error<string>>> constructorFactory, Func<T, TPrimitiveType> valueProvider)
    {
        _constructorFactory = constructorFactory;
        _valueProvider = valueProvider;
    }

    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = _constructorFactory(GetValue(ref reader));

        return result.IsT0 ? result.AsT0 : throw new JsonException(result.AsT1.Value);
    }


    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var valueConverter = (JsonConverter<TPrimitiveType>)options.GetConverter(typeof(TPrimitiveType));
        valueConverter.Write(writer, _valueProvider(value), options);
    }

    protected abstract TPrimitiveType? GetValue(ref Utf8JsonReader reader);
}