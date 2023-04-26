using System.Text.Json;
using Code.Infrastructure.Json.Converters;

namespace Code.Infrastructure.Json;

public static class JsonSerializerWrapper
{
    private static readonly JsonSerializerOptions Options;

    static JsonSerializerWrapper()
    {
        Options = new JsonSerializerOptions();
        Options.Converters.Add(new ReadOnlyCollectionJsonConvertFactory());
    }

    public static string Serialize(object obj) => JsonSerializer.Serialize(obj, Options);

    public static T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}