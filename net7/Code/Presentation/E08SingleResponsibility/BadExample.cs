using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Code.Presentation.E08SingleResponsibility;

public sealed class BadExample
{
    public sealed class Service
    {
        private readonly ILogger _logger;
        public Service(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public Task<string> GetValue()
        {
            return RetryForever(GetValueInt);
        }

        private Task<string> GetValueInt()
        {
            return Task.FromResult("value");
        }

        private async Task<T> RetryForever<T>(Func<Task<T>> func)
        {
            while (true)
            {
                try
                {
                    return await func();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Sth happened");
                }
            }
        }
    }

    public sealed class Consumer
    {
        private readonly Service _service;

        public Consumer(ILoggerFactory loggerFactory)
        {
            _service = new Service(loggerFactory);
        }

        public async Task Execute()
        {
            var result = await _service.GetValue();
            Debug.WriteLine(result);

            return;
        }
    }

}