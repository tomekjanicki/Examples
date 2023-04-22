using System.Text.Json;
using Code.Infrastructure.Json.Converters;

namespace Code.Infrastructure.Json;

public static class SerializerWrapper
{
    private static readonly JsonSerializerOptions Options;

    static SerializerWrapper()
    {
        Options = new JsonSerializerOptions();
        Options.Converters.Add(new ReadOnlyCollectionJsonConvertFactory());
    }

    public static string Serialize(object obj) => JsonSerializer.Serialize(obj, Options);

    public static T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value, Options);
}