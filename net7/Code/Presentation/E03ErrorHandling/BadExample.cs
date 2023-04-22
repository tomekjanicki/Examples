using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E03ErrorHandling;

public sealed class BadExample
{
    public sealed class Service
    {
        private readonly ILogger _logger;

        public Service(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public void Process(int? value)
        {
            try
            {
#pragma warning disable CS8629
                ProcessInt(value.Value);
#pragma warning restore CS8629
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Something wrong happened");
            }
        }

        private void ProcessInt(int value)
        {
            Debug.WriteLine(value);
        }
    }
}