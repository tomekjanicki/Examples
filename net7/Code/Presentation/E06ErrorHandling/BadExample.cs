using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E06ErrorHandling;

public sealed class BadExample
{
    public sealed class DeserializerWrapper
    {
        private readonly ILogger _logger;

        public DeserializerWrapper(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public T? DeserializeV1<T>(string jsonString)
            where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch
            {
                return null;
            }
        }

        public T? DeserializeV2<T>(string jsonString)
            where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sth wrong happened");

                return null;
            }
        }

        public T? DeserializeV3<T>(string jsonString)
            where T : class
        {
            try
            {
                return JsonSerializer.Deserialize<T>(jsonString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sth wrong happened");

                throw;
            }
        }
    }
}