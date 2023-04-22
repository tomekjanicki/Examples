using System.Text.Json;
using OneOf;

namespace Code.Presentation.E06ErrorHandling;

public sealed class GoodExample
{
    public sealed class DeserializerWrapper
    {
        //fail fast, provide as much context information (what is wrong) as possible,
        //never mute the exception and pretend that everything was fine
        //never mask the real problem - in BadExample.DeserializeV1 we mask JsonException with NullReferenceException
        //when something is optional the caller should decide if the conversion fail that it can ignore and continue without the data

        public OneOf<T?, JsonException> DeserializeV1<T>(string jsonString)
            where T : class
        {
            return JsonSerializer.Deserialize<T>(jsonString);
        }

        public OneOf<T?, JsonException> DeserializeV2<T>(string jsonString)
            where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (JsonException ex)
            {
                return ex;
            }
        }
    }
}