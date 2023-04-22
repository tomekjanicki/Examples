using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E03ErrorHandling;

public sealed class GoodExample
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
            //exceptions should be used for exceptional cases, when we expect something to happen via object signature we should handle it.
            //exceptions really affect performance of the app
            //exceptions should not control the program flow - OperationSuccessfullyException
            if (value is null)
            {
                _logger.LogError("Value was null");

                return;
            }
            ProcessInt(value.Value);
        }

        private void ProcessInt(int value)
        {
            Debug.WriteLine(value);
        }
    }
}