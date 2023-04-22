using System.Text.Json;
using OneOf;
using OneOf.Types;

namespace Code.Infrastructure.Json.Converters.Abstractions;

public abstract class Int32BasedJsonConvert<T> : StructBasedJsonConvert<T, int>
    where T : struct
{
    protected Int32BasedJsonConvert(Func<int?, OneOf<T, Error<string>>> constructorFactory, Func<T, int> valueProvider) 
        : base(constructorFactory, valueProvider)
    {
    }

    protected override int? GetValue(ref Utf8JsonReader reader) => reader.TokenType == JsonTokenType.Number ? reader.GetInt32() : null;
}