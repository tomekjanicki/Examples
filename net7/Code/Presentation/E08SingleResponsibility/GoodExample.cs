using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Code.Presentation.E08SingleResponsibility;

public sealed class GoodExample
{
    public sealed class Retry
    {
        private readonly ILogger _logger;

        public Retry(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task<T> RetryForever<T>(Func<Task<T>> func)
        {
            while (true)
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Sth happened");
                }
            }
        }
    }

    public interface IService
    {
        Task<string> GetValue();
    }

    public sealed class ServiceRetry : IService
    {
        private readonly IService _service;
        private readonly Retry _retry;

        public ServiceRetry(IService service, Retry retry)
        {
            _service = service;
            _retry = retry;
        }
        public Task<string> GetValue()
        {
            return _retry.RetryForever(_service.GetValue);
        }
    }

    public sealed class Service : IService
    {
        public Task<string> GetValue()
        {
            return GetValueInt();
        }

        private Task<string> GetValueInt()
        {
            return Task.FromResult("value");
        }
    }

    public sealed class Consumer
    {
        private readonly IService _service;

        public Consumer(ILoggerFactory loggerFactory)
        {
            _service = new ServiceRetry(new Service(), new Retry(loggerFactory));
        }


        public async Task Execute()
        {
            var result = await _service.GetValue();
            Debug.WriteLine(result);

            return;
        }
    }
}